using Amazon.SecretsManager;
using Castle.MicroKernel;
using Castle.MicroKernel.Handlers;
using Castle.Windsor;
using Castle.Windsor.Diagnostics;
using Castle.Windsor.MsDependencyInjection;
using GraphQL;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Scrutor;
using Calendar.API.Configuration;
using Calendar.API.Factories;
using Calendar.DAL;
using Calendar.DAL.Interfaces;
using Calendar.DAL.Repositories;
using System;
using System.Linq;
using System.Reflection;
using System.Text;
using SecretManager = Calendar.Domain.Services.SecretManager;

namespace Calendar.API.Extensions
{
    public static class CalendarDiServicesExtension
    {
        private static void AssertDiConfiguration(WindsorContainer container)
        {
            var host = (IDiagnosticsHost)container.Kernel.GetSubSystem(SubSystemConstants.DiagnosticsKey);
            var misconfigurations = host.GetDiagnostic<IPotentiallyMisconfiguredComponentsDiagnostic>().Inspect();

            if (misconfigurations.Any())
            {
                var diagnosticOutput = new StringBuilder();

                foreach (IExposeDependencyInfo problem in misconfigurations)
                {
                    problem.ObtainDependencyDetails(new DependencyInspector(diagnosticOutput));
                }

                throw new Exception(diagnosticOutput.ToString());
            }
        }

        private static WindsorContainer _container = new WindsorContainer();

        public static void AddCalendarServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Infrastructure

            services.AddCors();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services
                .AddMiniProfiler(options => { options.RouteBasePath = "/profiler"; })
                .AddEntityFramework();

            // AWS

            services.AddSingleton<SecretManager>();
            services.AddDefaultAWSOptions(configuration.GetAWSOptions());
            services.AddAWSService<IAmazonSecretsManager>();

            var sp = WindsorRegistrationHelper.CreateServiceProvider(_container, services);
            var sm = sp.GetService<SecretManager>();

            // Authorization & Authentication

            services.AddAuthorization(auth =>
            {
                auth.AddPolicy("JwtBearer", new AuthorizationPolicyBuilder()
                    .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme‌​)
                    .RequireAuthenticatedUser()
                    .Build());
            });

            var cognitoSettings = sm.GetCognitoOptions().Result;

            var s = sm.GetConnectionString().Result;

            services
                .AddAuthentication(opt => { opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme; })
                .AddJwtBearer(opt =>
                {
                    opt.SaveToken = true;
                    opt.Audience = cognitoSettings.ClientId;
                    opt.Authority = $"https://cognito-idp.us-east-2.amazonaws.com/{cognitoSettings.UserPoolId}";
                    opt.TokenValidationParameters = 
                    JwtHelper.TokenValidationParameters(cognitoSettings.RSAModulus, cognitoSettings.RSAExponent, cognitoSettings.JwtIssuer);
                        // new TokenValidationParameters
                        // {
                        //     RoleClaimType = "cognito:groups",
                        // };
                });

            //services.AddAuthentication().AddJwtBearer(
            //    (jwtBearerOptions) => {
            //        jwtBearerOptions.TokenValidationParameters = JwtHelper.TokenValidationParameters(
            //            cognitoSettings.RSAModulus, cognitoSettings.RSAExponent, cognitoSettings.JwtIssuer);
            //    });

            // Clients

            services.AddSingleton(x => sm.GetCognitoOptions().Result);
            services.AddSingleton<ICalendarFactory, CalendarFactory>();
            services.AddSingleton<ICalendarFactory, CalendarFactory>();
            services.AddSingleton(x => x.GetService<ICalendarFactory>().CreateCognitoClient());
            services.AddSingleton(x => x.GetService<ICalendarFactory>().GetCognitoUserPool());

            // Repositories & services

            var connectionString = configuration.GetConnectionString("Calendar");
            services.AddSingleton(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddSingleton(typeof(IUserOwnedGenericRepository<>), typeof(UserOwnedGenericRepository<>));
            services.AddSingleton<IUserRepository, UserRepository>();
            services.AddDbContext<CalendarDbContext>();

            // GraphQL

            services.AddHttpContextAccessor();
            services.AddSingleton<DocumentExecuter>();

            services.AddSingleton(x => {
                var sp1 = WindsorRegistrationHelper.CreateServiceProvider(_container, services);
                return x.GetService<ICalendarFactory>().CreatePublicSchema(sp1);
            });

            // Auto registration

            services.Scan(scan => scan
            .FromAssemblies(
                Assembly.GetAssembly(typeof(CalendarFactory)),
                Assembly.GetAssembly(typeof(CalendarDbContext)))
                    .AddClasses()
                    .UsingRegistrationStrategy(RegistrationStrategy.Skip)
                    .AsSelf()
                    .WithSingletonLifetime());

            // Tests

            AssertDiConfiguration(_container);
        }
    }
}
