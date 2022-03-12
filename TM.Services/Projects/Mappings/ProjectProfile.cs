using AutoMapper;
using TM.Core.Entities;
using TM.Services.Projects.Resp;
using TM.Services.Projects.Rqst;

namespace TM.Services.Projects.Mappings
{
    /// <summary>
    /// Project Entity to dto mapper
    /// </summary>
    public class ProjectProfile : Profile
    {
        /// <summary>
        /// Ctor
        /// </summary>
        public ProjectProfile()
        {
            CreateMap<Project, GetProjectResp>();
            CreateMap<AddProjectRqst, Project>();
        }
    }
}
