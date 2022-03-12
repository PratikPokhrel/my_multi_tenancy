using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using my_multi_tenancy.Data.SwaggerConfig;
using System;
using System.IO;
using System.Reflection;
using Infrastructure.Filters;
using Infrastructure.Ioc;
using my_multi_tenancy.FIlters;
using Infrastructure.Middlewares;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Collections.Generic;
using TM.Services.Ioc;

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
            services.AddMvc(opt =>
            {
                opt.Filters.Add(typeof(ModelStateValidatorAttribute));
            });
            services.AddCors();
            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
            services.AddScoped<IsUserInTenant>();
            //services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie();

            services.ConfigureSwagger();
            //services.AddAndMigrateTenantDatabases();
            services.ConfigureAppServices(Configuration);
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.ConfigureTMServices();

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
            app.UseCors(x => x.AllowAnyHeader().AllowAnyMethod().WithOrigins("http://localhost:3000"));
            //app.ApplyUserKeyValidation();
            app.UseHttpsRedirection();
            app.UseMiddleware<ExceptionHandleMiddleware>();
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


                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme (Example: 'Bearer 12345abcdef')",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    Array.Empty<string>()
                }
            });

                var xmlCommentsFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlCommentsFullPath = Path.Combine(AppContext.BaseDirectory, xmlCommentsFile);
                c.IncludeXmlComments(xmlCommentsFullPath);
            });
        }

    }
}
