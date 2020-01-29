using Amazon;
using Amazon.CognitoIdentityProvider;
using Amazon.Extensions.CognitoAuthentication;
using GraphQL;
using Calendar.API.Models.Public;
using System;
using Calendar.Domain.Options;

namespace Calendar.API.Factories
{
    public interface ICalendarFactory 
    {
        CognitoUserPool GetCognitoUserPool();
        AmazonCognitoIdentityProviderClient CreateCognitoClient();
        IPublicSchema CreatePublicSchema(IServiceProvider sp);
    }

    public class CalendarFactory : ICalendarFactory
    {
        private readonly CognitoOptions _cognitoOptions;

        public CalendarFactory(CognitoOptions cognitoOptions)
        {
            _cognitoOptions = cognitoOptions;
        }
        
        public IPublicSchema CreatePublicSchema(IServiceProvider sp)
        {
            return new PublicCalendarSchema(new FuncDependencyResolver(type => 
            {
                return sp.GetService(type);
            }));
        }

        public AmazonCognitoIdentityProviderClient CreateCognitoClient()
        {
            var client = new AmazonCognitoIdentityProviderClient(RegionEndpoint.USEast2);
            return client;
        }

        public CognitoUserPool GetCognitoUserPool()
        {
            var client = CreateCognitoClient();
            var userPool = new CognitoUserPool(_cognitoOptions.UserPoolId, _cognitoOptions.ClientId, client);

            return userPool;
        }
    }
}
