using System;

namespace Calendar.Domain
{
    public interface ISafeDeletableDbElement : IDbElement
    {
        DateTime? DateDeleted { get; set; }
    }
}
