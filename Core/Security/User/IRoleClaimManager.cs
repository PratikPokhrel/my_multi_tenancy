using Core.Dto;
using Core.Dto.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Security.User
{
    public interface IRoleClaimManager
    {
        Task<Result> AddAsync(AppRoleClaim appRoleClaim);
    }
}
