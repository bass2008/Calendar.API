namespace Calendar.Domain.Options
{
    public class CognitoOptions
    {
        public string ClientId { get; set; }
        public string UserPoolId { get; set; }
        public string JwtIssuer { get; set; }
        public string RSAModulus { get; set; }
        public string RSAExponent { get; set; }
    }
}
