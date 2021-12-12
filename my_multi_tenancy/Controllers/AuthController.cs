using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
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

        public AuthController(IHttpContextAccessor httpContextAccessor)
        {
            _contextAccessor = httpContextAccessor;
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
        private static ClaimsIdentity CreateIdentity(AppUser user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, "4249f843-d4a3-4d9c-b0ff-bc1a9d3cd5e1"),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Email, user.UserName)
            };
            var claimsIdentity = new ClaimsIdentity(claims,"custom");
            return claimsIdentity;
        }
    }

    public class AppUser
    {
        public string UserName { get; set; }
        public string Email { get; set; }
    }
}
