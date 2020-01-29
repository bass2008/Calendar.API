using GraphQL.Types;
using Calendar.API.Decorators;
using Calendar.DAL.Interfaces;
using Calendar.Domain;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace Calendar.API.Models.Common
{
    public class BaseCalendarQuery : ObjectGraphType
    {
        protected readonly CalendarRequestDecorator _requestDecorator;

        public BaseCalendarQuery(CalendarRequestDecorator requestDecorator)
        {
            _requestDecorator = requestDecorator;
        }

        public abstract class BaseGraphQLFilter<TDomain> where TDomain : class, IDbElement
        {
            public QueryArgument QueryArgument { get; set; }

            public abstract Expression<Func<TDomain, bool>> GetFilter(ResolveFieldContext<object> context);
        }

        /// <summary>
        /// Add single user owned field like UserPrefeneces
        /// </summary>
        protected void AddSingleFieldAsync<TDomain, TGraphQL>(string singleName, IUserOwnedGenericRepository<TDomain> repository)
            where TDomain : class, IUserOwnedElement
            where TGraphQL : ObjectGraphType<TDomain>
        {
            FieldAsync<NonNullGraphType<TGraphQL>>(
                singleName,
                resolve: async resolveContext =>
                {
                    return await _requestDecorator.Run(resolveContext, async context =>
                    {
                        return await repository.GetSingleUserOwnedAsync();
                    });
                });
        }

        /// <summary>
        /// Add common entity with single and plural name
        /// </summary>
        protected void AddFieldAsync<TDomain, TGraphQL>(string singleName, string pluralName, IGenericRepository<TDomain> repository)
            where TDomain : class, IDbElement
            where TGraphQL : ObjectGraphType<TDomain>
        {
            AddFieldAsync<TDomain, TGraphQL>(singleName, pluralName, repository, null);
        }

        /// <summary>
        /// Add common entity with single and plural name
        /// </summary>
        protected void AddFieldAsync<TDomain, TGraphQL>(string singleName, string pluralName, IGenericRepository<TDomain> repository, BaseGraphQLFilter<TDomain>[] filters = null) 
            where TDomain : class, IDbElement
            where TGraphQL: ObjectGraphType<TDomain>
        {
            var countArgs = new List<QueryArgument>();

            var args = new List<QueryArgument>();
            args.Add(new QueryArgument<IntGraphType> { Name = "pageSize", DefaultValue = 30 });
            args.Add(new QueryArgument<IntGraphType> { Name = "page", DefaultValue = 1 });

            if (filters != null)
                foreach (var filter in filters)
                {
                    args.Add(filter.QueryArgument);
                    countArgs.Add(filter.QueryArgument);
                }
            
            FieldAsync<NonNullGraphType<IntGraphType>>(
                $"{pluralName}Count",
                arguments: new QueryArguments(countArgs),
                resolve: async resolveContext => {
                    return await _requestDecorator.Run(resolveContext, async context =>
                    {


                        //foreach (var filter in filters)
                        //    if (context.HasArgument(filter.QueryArgument.Name))
                        //        return await repository.CountAsync(filter.Filter);

                        return await repository.CountAsync();
                    });
                });

            FieldAsync<NonNullGraphType<ListGraphType<NonNullGraphType<TGraphQL>>>>(
                pluralName,
                arguments: new QueryArguments(args),
                resolve: async resolveContext => {
                    return await _requestDecorator.Run(resolveContext, async context =>
                    {
                        var pageSize = context.GetArgument<int>("pageSize");
                        var page = context.GetArgument<int>("page");

                        if(filters != null)
                            foreach (var filter in filters)
                                if (context.HasArgument(filter.QueryArgument.Name))
                                    return await repository.GetAllAsync(page, pageSize, filter.GetFilter(context));

                        return await repository.GetAllAsync(page, pageSize);
                    });
                });

            FieldAsync<NonNullGraphType<TGraphQL>>(
                singleName,
                arguments: new QueryArguments(new QueryArgument<NonNullGraphType<IntGraphType>> { Name = "id" }),
                resolve: async resolveContext =>
                {
                    return await _requestDecorator.Run(resolveContext, async context =>
                    {
                        var id = context.GetArgument<int>("id");
                        return await repository.GetAsync(id);
                    });                                            
                });
        }
    }
}
