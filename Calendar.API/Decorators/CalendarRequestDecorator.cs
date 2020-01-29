using Amazon.Runtime;
using GraphQL;
using GraphQL.Types;
using Newtonsoft.Json;
using Calendar.DAL.Services;
using Calendar.Domain.Exceptions;
using System;
using System.Threading.Tasks;

namespace Calendar.API.Decorators
{
    public class CalendarRequestDecorator
    {
        private readonly PermissionService _permissionService;

        public CalendarRequestDecorator(PermissionService permissionService)
        {
            _permissionService = permissionService;
        }

        public async Task<object> Run(
            ResolveFieldContext<object> context,
            Func<ResolveFieldContext<object>, Task<object>> func)
        {
            return await Run(context, func, async (ps) => { });
        }

        public async Task<object> Run(
            ResolveFieldContext<object> context, 
            Func<ResolveFieldContext<object>, Task<object>> func, 
            Func<PermissionService, Task> permissionCheckFunc)
        {
            var errorCode = CalendarErrors.ShowErrorToUser;
            string message;
            try
            {
                //#if !DEBUG
                //await permissionCheckFunc(_permissionService);
                //#endif
                return await func(context);
            }
            catch (AmazonServiceException ex) when (ex.ErrorCode == "UserNotConfirmedException")
            {
                errorCode = CalendarErrors.UserNotConfirmed;
                message = "The user is not confirmed";
            }
            catch (AmazonServiceException ex)
            {
                errorCode = CalendarErrors.ShowErrorToUser;
                message = ex.Message;
            }
            catch (CalendarNewPasswordRequiredException ex)
            {
                errorCode = CalendarErrors.NewPasswordRequired;
                message = ex.Message;
            }
            catch (CalendarInternalException ex)
            {
                errorCode = CalendarErrors.InternalError;
                message = ex.Message;
            }
            catch (CalendarException ex)
            {
                errorCode = CalendarErrors.ShowErrorToUser;
                message = ex.Message;
            }
            catch (CalendarNonAuthorizedException ex)
            {
                errorCode = CalendarErrors.NonAuthorized;
                message = ex.Message;
            }
            catch (Exception ex)
            {
                message = "An unknown error has occurred. ";
                errorCode = CalendarErrors.InternalError;
//#if DEBUG
                message += $"Exception: {ex.GetType().Name}, Message: {ex.Message}, StackTrace: {ex.StackTrace}";
//#endif
            }

            var obj = new CalendarGraphQLError(errorCode, message);
            var s = JsonConvert.SerializeObject(obj);
            context.Errors.Add(new ExecutionError(s));
            return null;
        }
    }
}
