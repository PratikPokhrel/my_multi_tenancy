using AutoMapper;
using System.Linq.Expressions;
using TM.Core.Entities;
using TM.Services.Issues.Resp;
using TM.Services.Issues.Rqst;

namespace TM.Services.Issues.Mappings
{
    /// <summary>
    /// Issue  Entity to dto mapper
    /// </summary>
    public class IssueProfile : Profile
    {
        /// <summary>
        /// Ctor
        /// </summary>
        public IssueProfile()
        {
            CreateMap<Issue, IssueResp>()
            .ForMember(vm => vm.ProjectId, m => m.MapFrom(mm => mm.Project != null ? mm.Project.Id : 0))
            .ForMember(vm => vm.ProjectName, m => m.MapFrom(mm => mm.Project != null ? mm.Project.Name : string.Empty));

            CreateMap<Issue, AddIssueRqst>().ReverseMap();
        }
    }
    public static class MappingExtension
    {
        public static IMappingExpression<TSource, TDestination> Ignore<TSource, TDestination>(
        this IMappingExpression<TSource, TDestination> map,
        Expression<Func<TDestination, object>> selector)
        {
            map.ForMember(selector, config => config.Ignore());
            return map;
        }
    }
}
