using Microsoft.EntityFrameworkCore;
using Calendar.Domain.Models;
using System;
using Calendar.Domain.Services;

namespace Calendar.DAL
{
    public class CalendarDbContext : DbContext
    {
        private readonly SecretManager _secretManager;
        
        public CalendarDbContext(SecretManager secretManager)
        {
            _secretManager = secretManager;
        }

        public DbSet<User> Users { get; set; }

        public DbSet<UserPreference> UserPreferences { get; set; }

        public DbSet<Tab> Tabs { get; set; }

        public DbSet<Event> Events { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {       
           
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            Console.WriteLine("OnConfiguring");
        
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            
            if (environment == "Testing")
            {
                optionsBuilder.UseInMemoryDatabase(databaseName: "TestInMemoryEfDatabase");
                return;
            }
            
            var connectionString = _secretManager.GetConnectionString().Result;
            
            Console.WriteLine($"Try connect to: {connectionString}");
            
            optionsBuilder.UseNpgsql(connectionString);
        }
    }   
}
