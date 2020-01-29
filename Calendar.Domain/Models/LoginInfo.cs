namespace Calendar.Domain.Models
{
    public class LoginInfo
    {
        public string Token { get; set; }

        public string ExpiresAt { get; set; }

        public User User { get; set; }
    }
}
