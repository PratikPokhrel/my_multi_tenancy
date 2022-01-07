using Core.EF.Data.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using LazyCache;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Newtonsoft.Json;
using Core.Dto;
using Microsoft.AspNetCore.Http;
using System.Net;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using Infrastructure.Settings;

namespace Infrastructure.Ioc
{
    public static class StartUp
    {
        public static void ConfigureAppServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.ConfigureService(configuration);
            services.IdentityConfiguration();
            services.ConfigureEFService(configuration);
            services.ConfigureCommonService(configuration);
            services.AddJwtAuthentication();
            services.AddLazyCache();
        }

        internal static IServiceCollection AddJwtAuthentication(
            this IServiceCollection services)
        {

            var appSettings = services.BuildServiceProvider().GetRequiredService<AuthenticationSettings>();

            var key = Encoding.ASCII.GetBytes(appSettings.Secret);

            services
                .AddAuthentication(authentication =>
                {
                    authentication.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    authentication.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(async bearer =>
                {
                    bearer.RequireHttpsMetadata = false;
                    bearer.SaveToken = true;
                    bearer.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        RoleClaimType = ClaimTypes.Role,
                        ClockSkew = TimeSpan.Zero
                    };


                    bearer.Events = new JwtBearerEvents
                    {
                        OnAuthenticationFailed = c =>
                        {
                            if (c.Exception is SecurityTokenExpiredException)
                            {
                                c.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                                c.Response.ContentType = "application/json";
                                var result = JsonConvert.SerializeObject("The Token is expired.");
                                return c.Response.WriteAsync(result);
                            }
                            else
                            {
                                c.NoResult();
                                c.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                                c.Response.ContentType = "text/plain";
                                return c.Response.WriteAsync(c.Exception.ToString());
                                c.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                                c.Response.ContentType = "application/json";
                                var result = JsonConvert.SerializeObject(Result.Fail("An unhandled error has occurred."));
                                return c.Response.WriteAsync(result);
                            }
                        },
                        OnChallenge = context =>
                        {
                            context.HandleResponse();
                            if (!context.Response.HasStarted)
                            {
                                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                                context.Response.ContentType = "application/json";
                                var result = JsonConvert.SerializeObject(Result.Fail("You are not Authorized."));
                                return context.Response.WriteAsync(result);
                            }

                            return Task.CompletedTask;
                        },
                        OnForbidden = context =>
                        {
                            context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                            context.Response.ContentType = "application/json";
                            var result = JsonConvert.SerializeObject(Result.Fail("You are not authorized to access this resource."));
                            return context.Response.WriteAsync(result);
                        },
                    };
                });

            return services;
        }
    }


}
