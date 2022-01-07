using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services.Identity.Resp
{
    public class TokenResponse
    {
        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="token"></param>
        /// <param name="refreshToken"></param>
        /// <param name="userImageUrl"></param>
        public TokenResponse(string token, string refreshToken, string userImageUrl)
        {
            Token = token;
            RefreshToken = refreshToken;
            UserImageURL = userImageUrl;
        }

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="token"></param>
        /// <param name="refreshToken"></param>
        /// <param name="refreshTokenExpireTime"></param>
        public TokenResponse(string token, string refreshToken, DateTime refreshTokenExpireTime)
        {
            Token = token;
            RefreshToken = refreshToken;
            RefreshTokenExpiryTime = refreshTokenExpireTime;
        }

        public string Token { get; set; }
        public string RefreshToken { get; set; }
        public string UserImageURL { get; set; }
        public DateTime RefreshTokenExpiryTime { get; set; }
    }
}
