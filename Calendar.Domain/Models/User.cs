using System;

namespace Calendar.Domain.Models
{
    public class User : IDbElement, IHasDateCreated
    {
        public int Id { get; set; }

        public string Email { get; set; }

        public string Name { get; set; }

        public string Phone { get; set; }

        public string Image { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime LastVisitDate { get; set; }

        public UserPreference UserPreference { get; set; }
    }
}
