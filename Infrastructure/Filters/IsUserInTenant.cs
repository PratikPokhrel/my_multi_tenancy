using Core.Dto;
using Core.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Services.Services;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace my_multi_tenancy.FIlters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class IsUserInTenant : ActionFilterAttribute
    {
        public override async void OnActionExecuting(ActionExecutingContext context)
        {
            ITenantService? tenantService = context.HttpContext.RequestServices.GetService<ITenantService>();
            if (tenantService == null)
                throw new Exception("");

            IHttpContextAccessor? httpContextAccessor = context.HttpContext.RequestServices.GetService<IHttpContextAccessor>();
            if (httpContextAccessor == null)
                throw new Exception("");

            if (httpContextAccessor == null || 
                httpContextAccessor.HttpContext == null ||
               !httpContextAccessor.HttpContext.User.Identities.SelectMany(e=>e.Claims).Any())
            {
                context.Result = new OkObjectResult(new { Success = false, Message = "Unauthorized" });
                return;
            }


            var userId = httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
            if (userId == null)
                throw new Exception("");

            if (!tenantService.IsUserInTenantAsync(Guid.Parse(userId.Value)).GetAwaiter().GetResult())
            {
                // dont continue
                context.Result =
                     new OkObjectResult(await Result.FailAsync("Not authorized to access".ToStringList()));
                return ;
            }
        }
    }
}
