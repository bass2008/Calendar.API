using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Calendar.Domain.Exceptions
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum CalendarErrors
    {
        ShowErrorToUser,
        NewPasswordRequired,
        UserNotConfirmed,
        InternalError,
        NonAuthorized
    }

    public class CalendarGraphQLError
    {
        public CalendarGraphQLError(CalendarErrors code, string message)
        {
            Code = code;
            Message = message;
        }

        public CalendarErrors Code { get; }

        public string Message { get; }
    }
}
