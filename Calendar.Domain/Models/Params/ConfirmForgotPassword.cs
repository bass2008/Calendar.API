namespace Calendar.Domain.Models.Params
{
    public class ConfirmForgotPassword
    {
        public string Email { get; set; }

        public string ConfirmationCode { get; set; }

        public string NewPassword { get; set; }
    }
}
