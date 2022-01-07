using AutoMapper;
using Core.Entities;
using Services.Services.Branches.Resp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services.Branches.Mappings
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
            CreateMap<BranchServiceResp, Branch>().ReverseMap();
        }
    }
}
