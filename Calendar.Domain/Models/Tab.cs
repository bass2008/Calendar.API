using System.Collections.Generic;

namespace Calendar.Domain.Models
{
    public class Tab : IDbElement, IUserOwnedElement
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Logo { get; set; }

        public int UserId { get; set; }

        public ICollection<Event> Events { get; set; }
    }
}
