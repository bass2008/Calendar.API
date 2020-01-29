namespace Calendar.Domain.Models
{
    public class UserPreference : IUserOwnedElement
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public User User { get; set; }

        public bool SmsChecked { get; set; }

        public bool EmailChecked { get; set; }

        public int Periodicity { get; set; }
    }
}
