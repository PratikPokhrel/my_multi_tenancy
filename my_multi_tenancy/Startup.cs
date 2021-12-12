using Core.EF.Data.Configuration;
using Core.EF.Data.Configuration.Pg;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using my_multi_tenancy.Data.SwaggerConfig;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Infrastructure.Ioc;
using my_multi_tenancy.Middlewares;
using my_multi_tenancy.FIlters;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace my_multi_tenancy
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
            services.AddControllers();
            services.AddScoped<IsUserInTenant>();
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie();
            services.ConfigureSwagger();
            //services.AddAndMigrateTenantDatabases();
            services.ConfigureAppServices(Configuration);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "my_multi_tenancy v1"));
            }
            //app.ApplyUserKeyValidation();
            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

           
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
    public static class SwaggerCOnfiguration
    {
        public static void ConfigureSwagger(this IServiceCollection services)
        {
            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(c =>
            {
            //https://thecodebuzz.com/jwt-authorization-token-swagger-open-api-asp-net-core-3-0/
            c.OperationFilter<AddSwaggerHeaderParameter>();
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "REST services for accounts.",
                    Version = "v1",
                    Description = "Through this API you can access accounts services",
                    Contact = new OpenApiContact()
                    {
                        Email = "dev@rigonepal.com",
                        Name = "Rigo Technologies  TenantId=5249f843-d4a3-4d9c-b0ff-bc1a9d3cd5e1",
                        Url = new Uri("https://rigonepal.com/")
                    }
                });
                var xmlCommentsFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlCommentsFullPath = Path.Combine(AppContext.BaseDirectory, xmlCommentsFile);
                //c.IncludeXmlComments(xmlCommentsFullPath);
            });
        }
        public static IServiceCollection AddAndMigrateTenantDatabases(this IServiceCollection services)
        {
            string connectionString = "User ID=postgres;Password=Admin;Host=localhost;Port=5432;Database=movies_db;";

            using var scope = services.BuildServiceProvider().CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<DeviceContext>();
            dbContext.Database.SetConnectionString(connectionString);
            if (dbContext.Database.GetMigrations().Count() > 0)
            {
                dbContext.Database.Migrate();
            }
            return services;
        }
    }
}
