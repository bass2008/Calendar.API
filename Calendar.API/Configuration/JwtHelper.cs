using Microsoft.IdentityModel.Tokens;
using System;
using System.Security.Cryptography;

namespace Calendar.API.Configuration
{
    public static class JwtHelper
    {
        public static RsaSecurityKey SigningKey(string modulus, string exponent)
        {
            using (var rrr = RSA.Create())
            {
                rrr.ImportParameters(
                    new RSAParameters()
                    {
                        Modulus = Base64UrlEncoder.DecodeBytes(modulus),
                        Exponent = Base64UrlEncoder.DecodeBytes(exponent)
                    }
                );

                return new RsaSecurityKey(rrr);
            }
        }

        public static TokenValidationParameters TokenValidationParameters(string modulus, string exponent, string issuer)
        {
            // Basic settings - signing key to validate with, audience and issuer.
            return new TokenValidationParameters
            {
                // Basic settings - signing key to validate with, IssuerSigningKey and issuer.
                IssuerSigningKey = SigningKey(modulus, exponent),

                // String represents the valid issuer
                ValidIssuer = issuer,

                // when receiving a token, check that the signing key
                ValidateIssuerSigningKey = true,

                // When receiving a token, check that we've signed it.
                ValidateIssuer = true,

                // When receiving a token, check that it is still valid.
                ValidateLifetime = true,

                // Do not validate Audience on the "access" token since Cognito does not supply it but it is on the "id"
                ValidateAudience = false,

                // This defines the maximum allowable clock skew - i.e. provides a tolerance on the token expiry time 
                // when validating the lifetime. As we're creating the tokens locally and validating them on the same 
                // machines which should have synchronised time, this can be set to zero. Where external tokens are
                // used, some leeway here could be useful.
                ClockSkew = TimeSpan.FromMinutes(0)
            };

        }
    }
}
