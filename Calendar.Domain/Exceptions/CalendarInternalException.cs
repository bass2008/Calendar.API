using System;

namespace Calendar.Domain.Exceptions
{
    public class CalendarInternalException : Exception
    {
        public CalendarInternalException(string message) : base(message) { }
    }
}
