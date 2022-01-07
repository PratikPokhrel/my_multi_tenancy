using Core.Dto;
using Microsoft.AspNetCore.Mvc;
using Services.Services.Auth;
using Services.Services.Identity.Rqst;
using System.Threading.Tasks;

namespace my_multi_tenancy.Controllers.Identity
{
    /// <summary>
    /// Authentication controller
    /// </summary>
    [Route("api/authentication")]
    public class AuthController : BaseApiController
    {
        private readonly IAuthService _authService;

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="authService"></param>
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        /// <summary>
        /// Token Api
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<Result>> SignInAsync([FromBody] TokenRequest dto)
        {
            var result=await _authService.LoginAsync(dto).ConfigureAwait(false);
            return AppOk(result);
        }

         /// <summary>
        /// Refresh Token
        /// </summary>
        /// <param name="model"></param>
        /// <returns>Status 200 OK</returns>
        [HttpPost("refresh")]
        public async Task<ActionResult> Refresh([FromBody] RefreshTokenRequest model)
        {
            var response = await _authService.GetRefreshTokenAsync(model);
            return Ok(response);
        }
    }


    /// <summary>
    /// Login Dto Model
    /// </summary>
    public class SigninDto
    {
        /// <summary>
        /// Username
        /// </summary>
        public string Username { get; set; }
        /// <summary>
        /// Password
        /// </summary>
        public string Password { get; set; }
    }
}
