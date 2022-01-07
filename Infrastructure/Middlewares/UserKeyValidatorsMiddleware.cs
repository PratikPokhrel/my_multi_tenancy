using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Services.Services;
using System.Threading.Tasks;

namespace Infrastructure.Middlewares
{
    public class UserKeyValidatorsMiddleware
    {
        private readonly RequestDelegate _next;
        public UserKeyValidatorsMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task Invoke(HttpContext context, ITenantService tenantService)
        {
            //if (!context.Request.Headers.Keys.Contains("user-key"))
            if (1 != 1)
            {
                context.Response.StatusCode = 400; //Bad Request                
                await context.Response.WriteAsync("User Key is missing");
                return;
            }
            else
            {
                if (!tenantService.IsUserInTenantAsync(Guid.Empty).GetAwaiter().GetResult())
                {
                    context.Response.StatusCode = 401; //UnAuthorized
                    await context.Response.WriteAsync("Invalid User Key");
                    return;
                }
            }
            await _next.Invoke(context);
        }
    }

    public static class UserKeyValidatorsExtension
    {
        public static IApplicationBuilder ApplyUserKeyValidation(this IApplicationBuilder app)
        {
            app.UseMiddleware<UserKeyValidatorsMiddleware>();
            return app;
        }
    }
}


