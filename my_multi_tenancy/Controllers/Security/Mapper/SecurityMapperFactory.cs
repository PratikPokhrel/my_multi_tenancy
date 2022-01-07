
using my_multi_tenancy.Controllers.Security.Dtos.Resp;
using Services.Services.Dtos.Rqst.Identity;

namespace my_multi_tenancy.Controllers.Security.Mappers
{
    public static class SecurityMapperFactory
    {
        public static Core.Dto.Security.AppUser ToAppUser(this UserRqst user)
        {
            return new Core.Dto.Security.AppUser
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                UserName = user.UserName,
                DOB = user.DOB,
                PhoneNumber=user.PhoneNumber,
                Roles = user.Roles,
            };
        }
    }
}
