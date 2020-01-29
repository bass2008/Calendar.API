namespace Calendar.Domain.Models.Params
{
    public class UserWithPassword
    {
        public string Email { get; set; }

        public string Password { get; set; }

        public string NewPasswordIfRequired { get; set; }
    }
}
