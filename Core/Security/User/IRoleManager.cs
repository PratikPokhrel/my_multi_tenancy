using Core.Dto;
using Core.Dto.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Core.Security.User
{
    public interface IRoleManager
    {
        Task<IEnumerable<AppRole>> GetAllAsync(string name, bool loadClaims);
        Task<Result> CreateAsync(AppRole role);
        Task<Result> UpdateAsync(AppRole role);
        Task<AppRole> FindByIdAsync(Guid id);
        Task<AppRole> FindByNameAsync(string name);
        Task<ApplicationIdentityResult> DeleteAsync(Guid id);
        Task<bool> ExistsAsync(string name,Guid? id = null);
        Task<IList<Claim>> GetClaimsAsync(AppRole role);
    }
}
