using System;

namespace Calendar.Domain.Exceptions
{
    public class CalendarNewPasswordRequiredException : Exception
    {
        private const string ShowMessage = "A new passwrod required for user";

        public CalendarNewPasswordRequiredException() : base(ShowMessage) { }
    }
}
