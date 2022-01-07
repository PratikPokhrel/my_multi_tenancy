using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Core.ClientInfo
{
    public class ClientInfoProvider : IClientInfoProvider
    {
        private readonly IHttpContextAccessor _context;

        public ClientInfoProvider(IHttpContextAccessor context)
        {
            _context = context;
        }

        public Guid UserId
        {
            get
            {

                if (_context != null &&
                    _context.HttpContext != null &&
                   _context.HttpContext.User.Identities.SelectMany(e => e.Claims).Any())
                {
                    var userId = _context.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                    return new Guid(userId);
                }
                return Guid.Empty;
            }
        }
    }
}
