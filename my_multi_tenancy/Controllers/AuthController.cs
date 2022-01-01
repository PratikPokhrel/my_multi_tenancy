using Core.Dto.Security;
using Core.EF.IdentityModels;
using Core.Infrastructure.Tenancy;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace my_multi_tenancy.Controllers
{
    [Route("api/Auth")]
    public class AuthController : Controller
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ITenantProvider _tenantProvider;

        public AuthController(IHttpContextAccessor httpContextAccessor, UserManager<ApplicationUser> userManager, ITenantProvider tenantProvider)
        {
            _contextAccessor = httpContextAccessor;
            _userManager = userManager;
            _tenantProvider = tenantProvider;
        }

        [HttpPost]
        public async Task<ActionResult> SignIn(AppUser appUser)
        {
            var authProperties = new AuthenticationProperties
            {
                IsPersistent = true,
                ExpiresUtc = DateTime.Now.AddMinutes(5000),
            };
            var userIdentity=  CreateIdentity(appUser);
            await _contextAccessor.HttpContext.SignInAsync("Cookies", new ClaimsPrincipal(userIdentity), authProperties).ConfigureAwait(false);
            return Ok(new { Success = true });

        }

        [HttpPost,Route("add-new")]
        public async Task<ActionResult> AddAsync()
        {
            var user = new ApplicationUser
            {
                FirstName = "Pratik",
                LastName = "Pokharel",
                PassWord = "Admin@123",
                Email = "ram@rigo.com",
                UserName = "ram@rigo.com",
                TenantId=_tenantProvider.TenantId
            };
            var identityResult=  await _userManager.CreateAsync(user).ConfigureAwait(false);
            return Ok(new { Success = true });

        }
        private static ClaimsIdentity CreateIdentity(AppUser user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, "2a26a99c-decc-46b4-ae07-6fab01ed7138"),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Email, user.UserName)
            };
            var claimsIdentity = new ClaimsIdentity(claims,"custom");
            return claimsIdentity;
        }
    }
    
}
