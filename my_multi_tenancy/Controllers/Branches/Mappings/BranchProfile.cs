using AutoMapper;
using Core.Entities;
using my_multi_tenancy.Controllers.Branches.Dtos;
using my_multi_tenancy.Controllers.Branches.Dtos.Rqst;
using Services.Services.Branches.Resp;

namespace my_multi_tenancy.Controllers.Branches.Mappings
{
    /// <summary>
    /// Branch Entity dto mapper
    /// </summary>
    public class BranchProfile : Profile
    {
        /// <summary>
        /// Ctor
        /// </summary>
        public BranchProfile()
        {
            CreateMap<BranchServiceResp, BranchResp>().ReverseMap();
            CreateMap<Branch, BranchDto>().ReverseMap();
        }
    }
}
