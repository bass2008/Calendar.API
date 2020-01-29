using System;

namespace Calendar.Domain
{
    public interface IHasDateProcessed
    {
        DateTime? DateProcessed { get; set; }
    }
}
