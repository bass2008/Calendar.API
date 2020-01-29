using System;

namespace Calendar.Domain.Exceptions
{
    public class CalendarNonAuthorizedException : Exception
    {
        private const string ShowMessage = "The user is not authorized for this operation";

        public CalendarNonAuthorizedException() : base(ShowMessage) { }

        public CalendarNonAuthorizedException(string message) : base(message) { }
    }
}
