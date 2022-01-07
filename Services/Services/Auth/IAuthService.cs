using Core.Dto;
using Services.Services.Identity.Resp;
using Services.Services.Identity.Rqst;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services.Auth
{
    public interface IAuthService
    {
        Task<Result<TokenResponse>> LoginAsync(TokenRequest model);
        Task<Result<TokenResponse>> GetRefreshTokenAsync(RefreshTokenRequest model);
    }
}
