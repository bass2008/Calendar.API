using Calendar.API.Extensions;
using Calendar.DAL;
using Calendar.Domain.Services;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;

namespace Calendar.API.Factories
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<CalendarDbContext>
    {
        public CalendarDbContext CreateDbContext(string[] args)
        {
            Console.WriteLine(Directory.GetCurrentDirectory());

            var services = CreateServiceCollection(args);
            var sp = services.BuildServiceProvider();
            var secretManager = sp.GetService<SecretManager>();

            return new CalendarDbContext(secretManager);
        }

        private static void ConfigureServiceCollection(IConfigurationRoot configuration, IServiceCollection services)
        {
            services.AddCalendarServices(configuration);
        }

        public static IServiceCollection CreateServiceCollection(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddCommandLine(args)
                .Build();

            Console.WriteLine("Configure DI");

            var services = new ServiceCollection();

            ConfigureServiceCollection(configuration, services);

            return services;
        }
    }
}
