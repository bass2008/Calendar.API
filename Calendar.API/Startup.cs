using GraphiQl;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Calendar.API.Extensions;
using Calendar.DAL;
using System.Linq;
using Calendar.Domain.Models;
using System;

namespace Calendar.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCalendarServices(Configuration);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, CalendarDbContext dbContext)
        {
            app.UseMiniProfiler();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseCors(x => x.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
            app.UseGraphiQl("/graphiql", "/graphql");
            app.UseMvc();

            SeedDb(dbContext);
        }

        private void SeedDb(CalendarDbContext dbContext)
        {
            if (dbContext.Users.Any() == false)
            {
                var email = "test@test.com";
                var user = new User
                {
                    Email = email,
                    DateCreated = DateTime.Now.ToUniversalTime(),
                    LastVisitDate = DateTime.Now.ToUniversalTime(),
                    Phone = "+7 951 111 11 11"
                };
                dbContext.Users.Add(user);
                dbContext.SaveChanges();
            }
        }
    }
}
