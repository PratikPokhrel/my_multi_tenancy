#region "namespace imports"
using Core;
using Core.Dto;
using Core.Dto.Security;
using Core.Security.User;
using Infrastructure.Settings;
using Microsoft.Extensions.Localization;
using Microsoft.IdentityModel.Tokens;
using Services.Services.Identity.Resp;
using Services.Services.Identity.Rqst;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
#endregion

namespace Services.Services.Auth
{
    /// <summary>
    /// Authentication service
    /// </summary>
    public class AuthService : IAuthService
    {
        #region "Ctor/properties"
        private readonly IApplictionUserManager _userManager;
        private readonly IStringLocalizer _localizer;
        private readonly IRoleManager _roleManager;
        private readonly AuthenticationSettings _authSettings;

        public AuthService(IApplictionUserManager userManager, IRoleManager roleManager, AuthenticationSettings authSettings, ICommonLocalizer commonLocalizer)
        {
            _userManager = userManager;
            _localizer = commonLocalizer.Localize;
            _roleManager = roleManager;
            _authSettings = authSettings;
        }
        #endregion


        #region "Service methods"
        public async Task<Result<TokenResponse>> LoginAsync(TokenRequest model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email).ConfigureAwait(false);

            var validateLogin = await ValidateLoginRequestAsync(model, user).ConfigureAwait(false);
            if (!validateLogin.Succeeded)
                return validateLogin;

            user.RefreshToken = await GenerateRefreshTokenAsync().ConfigureAwait(false);
            user.RefreshTokenExpiryTime = DateTime.Now.AddMinutes(10);
            await _userManager.UpdateAsync(user).ConfigureAwait(false);

            string token = await GenerateJwtTokenAsync(user).ConfigureAwait(false);
            var response = new TokenResponse(token, user.RefreshToken, string.Empty);
            return await Result<TokenResponse>.SuccessAsync(response);
        }

        private async Task<Result<TokenResponse>> ValidateLoginRequestAsync(TokenRequest model, AppUser user)
        {
            if (user == null)
                return await Result<TokenResponse>.FailAsync(_localizer["User Not Found."]);

            if (!user.IsActive)
                return await Result<TokenResponse>.FailAsync(_localizer["Your account is deactivated,please contact your administrator"]);

            var isValidPasswordValid = await _userManager.CheckPasswordAsync(user, model.Password).ConfigureAwait(false);
            if (!isValidPasswordValid)
                return await Result<TokenResponse>.FailAsync(_localizer["Invalid Credentials."]);

            return await Result<TokenResponse>.SuccessAsync();
        }

        public async Task<Result<TokenResponse>> GetRefreshTokenAsync(RefreshTokenRequest model)
        {
            if (model is null)
                return await Result<TokenResponse>.FailAsync("Invalid Client Token.");

            var userPrincipal = GetPrincipalFromExpiredToken(model.Token);
            var userEmail = userPrincipal.FindFirst(ClaimTypes.Email);

            if (userEmail == null)
                return await Result<TokenResponse>.FailAsync(_localizer["User Not Found."]);

            var user = await _userManager.FindByEmailAsync(userEmail.Value);
            if (user == null)
                return await Result<TokenResponse>.FailAsync(_localizer["User Not Found."]);

            if (user.RefreshToken != model.RefreshToken || user.RefreshTokenExpiryTime <= DateTime.Now)
                return await Result<TokenResponse>.FailAsync(_localizer["Invalid Client Token."]);

            string token = GenerateEncryptedToken(GetSigningCredentials(), await GetClaimsAsync(user).ConfigureAwait(false));
            user.RefreshToken = await GenerateRefreshTokenAsync().ConfigureAwait(false);
            await _userManager.UpdateAsync(user).ConfigureAwait(false);

            var response = new TokenResponse(token, user.RefreshToken, user.RefreshTokenExpiryTime);
            return await Result<TokenResponse>.SuccessAsync(response);
        }

        private async Task<string> GenerateJwtTokenAsync(AppUser user)
        {
            var token = GenerateEncryptedToken(GetSigningCredentials(), await GetClaimsAsync(user));
            return token;
        }

        private async Task<IEnumerable<Claim>> GetClaimsAsync(AppUser user)
        {
            IList<Claim> userClaims = await _userManager.GetClaimsAsync(user).ConfigureAwait(false);
            IList<string> roles = await _userManager.GetRolesAsync(user).ConfigureAwait(false);
            IList<Claim> roleClaims = new List<Claim>();
            List<Claim> permissionClaims = new List<Claim>();

            foreach (string role in roles)
            {
                roleClaims.Add(new Claim(ClaimTypes.Role, role));
                var thisRole = await _roleManager.FindByNameAsync(role).ConfigureAwait(false);
                var allPermissionsForThisRoles = await _roleManager.GetClaimsAsync(thisRole).ConfigureAwait(false);
                permissionClaims.AddRange(allPermissionsForThisRoles);
            }

            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new(ClaimTypes.Email, user.Email),
                new(ClaimTypes.Name, user.FirstName),
                new(ClaimTypes.Surname, user.LastName),
                new(ClaimTypes.MobilePhone, user.PhoneNumber ?? string.Empty)
            }
            .Union(userClaims)
            .Union(roleClaims)
            .Union(permissionClaims);

            return claims;
        }

        private static async Task<string> GenerateRefreshTokenAsync()
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return await Task.FromResult(Convert.ToBase64String(randomNumber));
        }

        private static string GenerateEncryptedToken(SigningCredentials signingCredentials, IEnumerable<Claim> claims)
        {
            var token = new JwtSecurityToken(
                          claims: claims,
                          expires: DateTime.UtcNow.AddMinutes(10),
                          signingCredentials: signingCredentials);

            var tokenHandler = new JwtSecurityTokenHandler();
            var encryptedToken = tokenHandler.WriteToken(token);
            return encryptedToken;
        }

        private ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_authSettings.Secret)),
                ValidateIssuer = false,
                ValidateAudience = false,
                RoleClaimType = ClaimTypes.Role,
                ClockSkew = TimeSpan.Zero,
                ValidateLifetime = false
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var securityToken);
            if (securityToken is not JwtSecurityToken jwtSecurityToken ||
                             !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Invalid token");

            return principal;
        }

        private SigningCredentials GetSigningCredentials()
        {
            var secret = Encoding.UTF8.GetBytes(_authSettings.Secret);
            return new SigningCredentials(new SymmetricSecurityKey(secret), SecurityAlgorithms.HmacSha256);
        }

        #endregion
    }
}
