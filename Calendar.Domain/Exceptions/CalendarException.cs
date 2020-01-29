using System;

namespace Calendar.Domain.Exceptions
{
    public class CalendarException : Exception
    {
        public CalendarException(string message) : base(message) { }
    }
}
