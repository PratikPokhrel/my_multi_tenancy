using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Services.Services;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace my_multi_tenancy.FIlters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class IsUserInTenant : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            ITenantService tenantService = context.HttpContext.RequestServices.GetService<ITenantService>();
            IHttpContextAccessor httpContextAccessor = context.HttpContext.RequestServices.GetService<IHttpContextAccessor>();

            if (httpContextAccessor == null || httpContextAccessor.HttpContext.User == null)
                context.Result = new OkObjectResult(new { Success = false, Message = "Unauthorized" });

            var userId = httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;

            if (tenantService.IsUserInTenant(Guid.Parse(userId)) == false)
            {
                // dont continue
                context.Result = new OkObjectResult(new { Success = false, Message = "My Bad" });
            }
        }
    }
}
