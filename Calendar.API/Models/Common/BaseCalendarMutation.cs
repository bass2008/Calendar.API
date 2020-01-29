using System;
using System.Reflection;
using GraphQL.Types;
using Calendar.API.Decorators;
using Calendar.API.Models.Entities;
using Calendar.API.Models.Inputs;
using Calendar.API.Services;
using Calendar.DAL.Interfaces;
using Calendar.Domain;
using Calendar.Domain.Models;
using Calendar.Domain.Models.Params;
using Calendar.DAL;
using System.Linq;
using Calendar.Domain.Exceptions;

namespace Calendar.API.Models.Common
{
    public class BaseCalendarMutation : ObjectGraphType
    {
        protected readonly CalendarRequestDecorator _requestDecorator;

        public BaseCalendarMutation(CalendarRequestDecorator requestDecorator)
        {
            _requestDecorator = requestDecorator;
        }

        protected void AddUserAsync(IUserRepository repository, CognitoService cognitoService, CalendarDbContext dbContext)
        {
            FieldAsync<NonNullGraphType<BooleanGraphType>>(
               "signUp",
               arguments: new QueryArguments(
                   new QueryArgument<NonNullGraphType<GraphQLUserSignUpInput>> { Name = "signUpData" }
               ),
               resolve: async resolveContext => {
                   return await _requestDecorator.Run(resolveContext, async context => {
                       var data = context.GetArgument<UserWithPassword>("signUpData");
                       await cognitoService.SignUpAsync(data.Email, data.Password);

                       var createData = context.GetArgument<User>("signUpData");
                       createData.DateCreated = DateTime.Now.ToUniversalTime();
                       createData.LastVisitDate = DateTime.Now.ToUniversalTime();

                       await repository.AddAsync(createData);

                       return true;
                   });
               });

            FieldAsync<NonNullGraphType<GraphQLLoginInfo>>(
                "login",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<GraphQLUserLoginInput>> { Name = "loginData" }
                ),
                resolve: async resolveContext => {
                    return await _requestDecorator.Run(resolveContext, async context => {
                        var data = context.GetArgument<UserWithPassword>("loginData");

                        // TODO: remove to repository
                        var user = dbContext.Users.SingleOrDefault(x => x.Email == data.Email);

                        if (user == null)
                            throw new CalendarException($"User with email: {data.Email} is not found");

                        var token = await cognitoService.LoginAsync(data.Email, data.Password, data.NewPasswordIfRequired);

                        await repository.UpdateLoginAsync(data.Email);

                        var loginInfo = new LoginInfo
                        {
                            User = user,
                            Token = token,
                            ExpiresAt = DateTime.Now
                                .ToUniversalTime()
                                .AddHours(1)
                                .Subtract(new DateTime(1970, 1, 1))
                                .TotalSeconds
                                .ToString()
                        };

                        return loginInfo;
                    });
                });

            FieldAsync<NonNullGraphType<BooleanGraphType>>(
                "confirmUser",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<GraphQLUserConfirmInput>> { Name = "confirmUser" }
                ),
                resolve: async resolveContext => {
                    return await _requestDecorator.Run(resolveContext, async context => {
                        var data = context.GetArgument<ConfirmUser>("confirmUser");

                        await cognitoService.ConfirmUserAsync(data.Email, data.ConfirmationCode);

                        return true;
                    });
                });

            FieldAsync<NonNullGraphType<BooleanGraphType>>(
                "changePassword",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<GraphQLUserChangePasswordInput>> { Name = "changePassword" }
                ),
                resolve: async resolveContext => {
                    return await _requestDecorator.Run(resolveContext, async context => {
                        var data = context.GetArgument<ChangePassword>("changePassword");

                        await cognitoService.ChangePasswordAsync(data.Token, data.OldPassword, data.NewPassword);

                        return true;
                    });
                });

            FieldAsync<NonNullGraphType<BooleanGraphType>>(
                "forgotPassword",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<GraphQLUserForgotPasswordInput>> { Name = "forgotPassword" }
                ),
                resolve: async resolveContext => {
                    return await _requestDecorator.Run(resolveContext, async context => {
                        var data = context.GetArgument<ForgotPassword>("forgotPassword");

                        await cognitoService.ForgotPasswordAsync(data.Email);

                        return true;
                    });
                });

            FieldAsync<NonNullGraphType<BooleanGraphType>>(
                "confirmForgotPassword",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<GraphQLUserConfirmForgotPasswordInput>> { Name = "confirmForgotPassword" }
                ),
                resolve: async resolveContext => {
                    return await _requestDecorator.Run(resolveContext, async context => {
                        var data = context.GetArgument<ConfirmForgotPassword>("confirmForgotPassword");

                        await cognitoService.ConfirmForgotPasswordAsync(data.Email, data.ConfirmationCode, data.NewPassword);

                        return true;
                    });
                });

            FieldAsync<NonNullGraphType<BooleanGraphType>>(
                "adminInviteUser",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<GraphQLAdminInviteUserInput>> { Name = "adminInviteUser" }
                ),
                resolve: async resolveContext => {
                    return await _requestDecorator.Run(resolveContext, async context => {
                        var data = context.GetArgument<AdminInviteUser>("adminInviteUser");

                        await cognitoService.AdminInviteUserAsync(data.Email);

                        return true;
                    });
                });

            FieldAsync<NonNullGraphType<BooleanGraphType>>(
                "adminChangeUserPassword",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<GraphQLAdminChangeUserPasswordInput>> { Name = "adminChangeUserPassword" }
                ),
                resolve: async resolveContext => {
                    return await _requestDecorator.Run(resolveContext, async context => {
                        var data = context.GetArgument<AdminChangeUserPassword>("adminChangeUserPassword");

                        await cognitoService.AdminChangeUserPassword(data.Email, data.Password);

                        return true;
                    });
                });

            FieldAsync<NonNullGraphType<IntGraphType>>(
                $"updateUser",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<IntGraphType>> { Name = "id" },
                    new QueryArgument<NonNullGraphType<GraphQLUserInput>> { Name = "updateData" }
                ),
                resolve: async resolveContext =>
                {
                    return await _requestDecorator.Run(resolveContext, async context =>
                    {
                        var id = context.GetArgument<int>("id");
                        var updateData = context.GetArgument<User>("updateData");

                        var dbUser = await repository.GetAsync(id);

                        dbUser.Image = updateData.Image;
                        dbUser.Phone = updateData.Phone;

                        await repository.UpdateAsync(id, updateData);
                        return id;
                    });
                });

            FieldAsync<NonNullGraphType<IntGraphType>>(
                $"deleteUser",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<IntGraphType>> { Name = "id" }
                ),
                resolve: async resolveContext => {
                    return await _requestDecorator.Run(resolveContext, async context => {
                        var id = context.GetArgument<int>("id");
                        await repository.DeleteAsync(id);
                        return id;
                    });
                });
        }

        /// <summary>
        /// Add single user owned field like UserPrefeneces
        /// </summary>
        protected void AddSingleFieldAsync<TDomain, TGraphQL, TInputType>(string singleName, IUserOwnedGenericRepository<TDomain> repository)
            where TDomain : class, IUserOwnedElement
            where TGraphQL : ObjectGraphType<TDomain>
            where TInputType : InputObjectGraphType
        {
            FieldAsync<NonNullGraphType<BooleanGraphType>>(
                $"addOrUpdate{singleName}",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<TInputType>> { Name = "updateData" }
                ),
                resolve: async resolveContext => {
                    return await _requestDecorator.Run(resolveContext, async context => {
                        var updateData = context.GetArgument<TDomain>("updateData");
                        await repository.AddOrUpdateSingleUserOwnedAsync(updateData);
                        return true;
                    });
                });
        }

        protected void AddMarkAsProcessedAsync<TDomain>(string name, IGenericRepository<TDomain> repository)
            where TDomain : class, IDbElement, IHasDateProcessed
        {
            FieldAsync<NonNullGraphType<IntGraphType>>(
                name,
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<IntGraphType>> { Name = "id" }
                ),
                resolve: async resolveContext => {
                    return await _requestDecorator.Run(resolveContext, async context => {
                        var id = context.GetArgument<int>("id");

                        var dbItem = await repository.GetAsync(id);
                        dbItem.DateProcessed = DateTime.Now.ToUniversalTime();
                        await repository.UpdateAsync(dbItem.Id, dbItem);

                        return id;
                    });
                });
        }

        protected void AddFieldAsync<TDomain, TGraphQL, TInputType>(string name, IGenericRepository<TDomain> repository)
            where TDomain : class, IDbElement
            where TGraphQL : ObjectGraphType<TDomain>
            where TInputType : InputObjectGraphType
        {
            FieldAsync<NonNullGraphType<IntGraphType>>(
                $"create{name}",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<TInputType>> { Name = "createData" }
                ),
               resolve: async resolveContext => {
                   return await _requestDecorator.Run(resolveContext, async context => {
                       var createData = context.GetArgument<TDomain>("createData");
                       await repository.AddAsync(createData);
                       return createData.Id;
                   });
               });

            FieldAsync<NonNullGraphType<IntGraphType>>(
                $"delete{name}",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<IntGraphType>> { Name = "id" }
                ),
                resolve: async resolveContext => {
                    return await _requestDecorator.Run(resolveContext, async context => {
                        var id = context.GetArgument<int>("id");
                        await repository.DeleteAsync(id);
                        return id;
                    });
                });

            FieldAsync<NonNullGraphType<IntGraphType>>(
                $"update{name}",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<IntGraphType>> { Name = "id" },
                    new QueryArgument<NonNullGraphType<TInputType>> { Name = "updateData" }
                ),
                resolve: async resolveContext => {
                    return await _requestDecorator.Run(resolveContext, async context => {
                        var id = context.GetArgument<int>("id");
                        var updateData = context.GetArgument<TDomain>("updateData");
                        await repository.UpdateAsync(id, updateData);
                        return id;
                    });
                });
        }
    }
}
