using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentityProvider.Model;
using Amazon.Extensions.CognitoAuthentication;
using System.Collections.Generic;
using System.Threading.Tasks;
using Calendar.Domain.Exceptions;
using Calendar.Domain.Options;

namespace Calendar.API.Services
{
    public class CognitoService
    {
        private CognitoUserPool _userPool;

        private readonly AmazonCognitoIdentityProviderClient _client;

        private readonly string _clientId;

        public CognitoService()// (AmazonCognitoIdentityProviderClient client, CognitoUserPool userPool, CognitoOptions options)
        {
            //_client = client;
            //_userPool = userPool;
            //_clientId = options.ClientId;
        }

        public async Task SignUpAsync(string email, string password)
        {
            // await _userPool.SignUpAsync(email, password, new Dictionary<string, string> { ["email"] = email }, new Dictionary<string, string>());
        }

        public async Task<string> LoginAsync(string email, string password, string newPasswordIfRequired)
        {
            return "ed136ccb-TEST-TOKEN-8b12c337aa2e";

            // var user = new CognitoUser(email, _clientId, _userPool, _client);
            // 
            // // TODO: create CognitoDevice here
            // //user.Device = new CognitoDevice(...)
            // 
            // var authRequest = new InitiateSrpAuthRequest()
            // {
            //     Password = password
            // };
            // 
            // var authResponse = await user.StartWithSrpAuthAsync(authRequest);
            // 
            // if (authResponse.ChallengeName?.Value == "NEW_PASSWORD_REQUIRED")
            // {
            //     if (string.IsNullOrWhiteSpace(newPasswordIfRequired))                
            //         throw new CalendarNewPasswordRequiredException();
            // 
            //     authResponse = await user.RespondToNewPasswordRequiredAsync(new RespondToNewPasswordRequiredRequest()
            //     {
            //         SessionID = authResponse.SessionID,
            //         NewPassword = newPasswordIfRequired
            //     });
            //     return authResponse.AuthenticationResult.AccessToken;
            // }
            // 
            // return authResponse.AuthenticationResult.AccessToken;
        }

        public async Task ConfirmUserAsync(string email, string confirmationCode)
        {
            // var user = new CognitoUser(email, _clientId, _userPool, _client);
            // 
            // await user.ConfirmSignUpAsync(confirmationCode, false);
        }

        public async Task ChangePasswordAsync(string token, string oldPassword, string newPassword)
        {
            // var request = new ChangePasswordRequest { AccessToken = token, PreviousPassword = oldPassword, ProposedPassword = newPassword };
            // await _client.ChangePasswordAsync(request);
        }

        public async Task ForgotPasswordAsync(string email)
        {
            // var user = new CognitoUser(email, _clientId, _userPool, _client);
            // 
            // await user.ForgotPasswordAsync();
        }

        public async Task ConfirmForgotPasswordAsync(string email, string confirmationCode, string newPassword)
        {
            // var user = new CognitoUser(email, _clientId, _userPool, _client);
            // 
            // await user.ConfirmForgotPasswordAsync(confirmationCode, newPassword);
        }

        public async Task AdminInviteUserAsync(string email)
        {
            // var request = new AdminCreateUserRequest
            // {
            //     UserPoolId = _userPool.PoolID,
            //     DesiredDeliveryMediums = new List<string> { "EMAIL" },
            //     Username = email,
            //     UserAttributes = new List<AttributeType>
            //     {
            //         new AttributeType { Name = "email", Value = email },
            //         new AttributeType { Name = "email_verified", Value = "True" }
            //     }
            // };
            // 
            // await _client.AdminCreateUserAsync(request);
        }

        public async Task AdminChangeUserPassword(string email, string password)
        {
            // var request = new AdminSetUserPasswordRequest
            // {
            //     UserPoolId = _userPool.PoolID,
            //     Username = email,
            //     Password = password,
            //     Permanent = true
            // };
            // 
            // await _client.AdminSetUserPasswordAsync(request);
        }
    }
}
