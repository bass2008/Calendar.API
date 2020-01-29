namespace Calendar.Domain.Models.Params
{
    public class ConfirmUser
    {
        public string Email { get; set; }

        public string ConfirmationCode { get; set; }
    }
}
