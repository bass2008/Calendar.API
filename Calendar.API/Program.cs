using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Sentry;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Compact;
using System;

namespace Calendar.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")?.ToLower();

            Console.WriteLine($"ASPNETCORE_ENVIRONMENT: {environment}");

            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false)
                .AddJsonFile($"appsettings.{environment}.json", optional: true)
                .AddEnvironmentVariables()
                .Build();

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .Enrich.FromLogContext()
                .WriteTo.Console(new CompactJsonFormatter())
                .CreateLogger();

            try
            {
                Log.Information("Starting web host");
                CreateWebHostBuilder(args).Build().Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseUrls("http://localhost:5200")
                .UseKestrel()
                .UseStartup<Startup>()
                .UseSerilog();
    }
}
