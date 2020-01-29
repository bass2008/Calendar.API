using System;
using System.Threading.Tasks;
using Amazon.SecretsManager;
using Amazon.SecretsManager.Model;
using Calendar.Domain.Common;
using Calendar.Domain.Options;

namespace Calendar.Domain.Services
{
    public class SecretManager
    {
        private readonly IAmazonSecretsManager _amazonSecretsManager;

        private string EnvironmentPrefix => GetShortEnvironmentName().ToString();

        private const string CognitoPrefix = "Cognito";

        private const string ConnectionStringKey = "ConnectionString";

        public SecretManager(IAmazonSecretsManager amazonSecretsManager)
        {
            _amazonSecretsManager = amazonSecretsManager;
        }

        private CalendarEnvironments GetShortEnvironmentName()
        {
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")?.ToLower();

            switch (environment)
            {
                case "development": return CalendarEnvironments.Dev;
                case "testing": return CalendarEnvironments.Test;
                case "stage": return CalendarEnvironments.Stage;
                case "production": return CalendarEnvironments.Prod;
                default: throw new ArgumentOutOfRangeException("Unknown environment value");
            }
        }

        public async Task SetConnectionString(string value, CalendarEnvironments environment)
        {
            await Set($"{environment}_{ConnectionStringKey}", value);
        }

        public async Task<string> GetConnectionString()
        {
              return "Host=localhost;Database=calendarApi;Username=postgres;Password=ci48kck3mDc2";
            //return "Host=localhost;Database=calendarApi;Username=root;Password=ci48kck3mDc2";

            //return await Get($"{EnvironmentPrefix}_{ConnectionStringKey}");
        }

        public async Task InitSecretManager(CognitoOptions cognitoOptions)
        {
            await SetCognitoOptions(cognitoOptions, CalendarEnvironments.Dev.ToString());
            await SetCognitoOptions(cognitoOptions, CalendarEnvironments.Test.ToString());
            await SetCognitoOptions(cognitoOptions, CalendarEnvironments.Stage.ToString());
            await SetCognitoOptions(cognitoOptions, CalendarEnvironments.Prod.ToString());
        }

        private async Task SetCognitoOptions(CognitoOptions options, string environmentPrefix) 
        {
            await Set($"{environmentPrefix}_{CognitoPrefix}_ClientId", options.ClientId);
            await Set($"{environmentPrefix}_{CognitoPrefix}_UserPoolId", options.UserPoolId);
            await Set($"{environmentPrefix}_{CognitoPrefix}_JwtIssuer", options.JwtIssuer);
            await Set($"{environmentPrefix}_{CognitoPrefix}_RSAModulus", options.RSAModulus);
            await Set($"{environmentPrefix}_{CognitoPrefix}_RSAExponent", options.RSAExponent);
        }

        public async Task<CognitoOptions> GetCognitoOptions()
        {
            return new CognitoOptions
            {
                ClientId = await Get($"{EnvironmentPrefix}_{CognitoPrefix}_ClientId"),
                UserPoolId = await Get($"{EnvironmentPrefix}_{CognitoPrefix}_UserPoolId"),
                JwtIssuer = await Get($"{EnvironmentPrefix}_{CognitoPrefix}_JwtIssuer"),
                RSAModulus = await Get($"{EnvironmentPrefix}_{CognitoPrefix}_RSAModulus"),
                RSAExponent = await Get($"{EnvironmentPrefix}_{CognitoPrefix}_RSAExponent")
            };
        }

        private async Task<string> Set(string key, string value)
        {
            var request = new CreateSecretRequest
            {
                Name = key,
                SecretString = value
            };

            try
            {
                var createResponse = await _amazonSecretsManager.CreateSecretAsync(request);
                return createResponse.VersionId;
            }
            catch (ResourceExistsException)
            {
                var updateRequest = new UpdateSecretRequest
                {
                    SecretId = key,
                    SecretString = value
                };

                var response = await _amazonSecretsManager.UpdateSecretAsync(updateRequest);
                return response.VersionId;
            }
        }
        
        public async Task<string> Get(string key)
        {
            var request = new GetSecretValueRequest
            {
                SecretId = key
            };

            var response = await _amazonSecretsManager.GetSecretValueAsync(request);
            return response.SecretString;
        }
    }
}
