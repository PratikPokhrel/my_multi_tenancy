using AutoMapper;
using Core;
using Core.ClientInfo;
using Core.Dto;
using Core.Infrastructure.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using TM.Core.Entities;
using TM.Services.Projects.Resp;
using TM.Services.Projects.Rqst;

namespace TM.Services.Projects
{
    public class ProjectService : IProjectService
    {
        #region "Ctor/properties"
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer _commonLocalizer;
        private readonly IClientInfoProvider _clientInfo;

        public ProjectService(IUnitOfWork  unitOfWork,IMapper mapper, ICommonLocalizer commonLocalizer,IClientInfoProvider clientInfo)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _commonLocalizer= commonLocalizer.Localize;
            _clientInfo = clientInfo;
        }



        #endregion


        #region "Service methods"
        public async Task<Result<GetProjectResp>> GetAsync()
        {
            var pz = await _unitOfWork.GetRepository<ProjectUser>()
                                    .Table
                                    .Include(e => e.Project)
                                    .ThenInclude(e => e.Issue)
                                    .FirstOrDefaultAsync(e => e.UserId == _clientInfo.UserId).ConfigureAwait(false);

            return (pz == null || pz.Project == null) ?
                 await Result<GetProjectResp>.FailAsync(_commonLocalizer["NotFound"]).ConfigureAwait(false) :
                   await Result<GetProjectResp>.SuccessAsync(_mapper.Map<GetProjectResp>(pz.Project)).ConfigureAwait(false);
        }



        public async Task<Result<GetProjectResp>> AddAsync(AddProjectRqst rqst)
        {
           var entity=_mapper.Map<Project>(rqst);
           var added= await _unitOfWork.GetRepository<Project>().AddAsync(entity).ConfigureAwait(false);
           await _unitOfWork.CommitAsync().ConfigureAwait(false);
           return await Result<GetProjectResp>.SuccessAsync(_mapper.Map<GetProjectResp>(added));
        }

        #endregion

    }
}
