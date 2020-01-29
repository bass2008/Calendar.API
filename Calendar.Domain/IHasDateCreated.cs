using System;

namespace Calendar.Domain
{
    public interface IHasDateCreated
    {
        DateTime DateCreated { get; set; }
    }
}
