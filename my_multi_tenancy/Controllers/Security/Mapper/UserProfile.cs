using AutoMapper;
using Core.Dto.Security;
using my_multi_tenancy.Controllers.Security.Dtos.Resp;
using System.Linq;

namespace my_multi_tenancy.Controllers.Security.Mapper
{
    public class UserProfile:Profile
    {
        public UserProfile()
        {
            CreateMap<UserListResp, AppUser>().ReverseMap()
                .ForMember(vm => vm.RoleName, m => m.MapFrom(mm => mm.Roles.Any() ? mm.Roles[0].Name : ""))
                .ForMember(vm => vm.RoleId, m => m.MapFrom(mm => mm.Roles.Any()? mm.Roles[0].Id : default));

        }
    }
}
