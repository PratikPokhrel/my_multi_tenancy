using AutoMapper;
using Core;
using Core.Dto;
using Core.Infrastructure;
using Core.Infrastructure.DataAccess;
using Core.Security.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using TM.Core.Entities;
using TM.Services.Issues.Resp;
using TM.Services.Issues.Rqst;

namespace TM.Services.Issues
{
    public class IssueService : IIssueService
    {
        #region "Ctor/properties"
        private readonly IRepository<Issue> _repository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IStringLocalizer _commonLocalizer;
        private readonly IApplictionUserManager _userManager;

        public IssueService(IUnitOfWork unitOfWork,IMapper mapper, ICommonLocalizer commonLocalizer,IApplictionUserManager userManager)
        {
            _repository = unitOfWork.GetRepository<Issue>();
            _mapper = mapper;
            _unitOfWork= unitOfWork;
            _commonLocalizer= commonLocalizer.Localize;
            _userManager = userManager;
        }
        #endregion


        #region "Service methods"
        public async Task<IEnumerable<IssueResp>> GetListAsync(int pzId,string searchText)
        {
            var query = _repository.GetAll()
                                  .Include(r => r.IssueUser)
                                  .Include(x => x.Project)
                                  .Where(e => (string.IsNullOrEmpty(searchText) || e.Description.ToLower().Contains(searchText.ToLower()) ||
                                                                                e.DescriptionText.ToLower().Contains(searchText.ToLower()))
                                                                                && e.ProjectId== pzId);
            var all=_userManager.GetAll();
            var qg=
                _repository.TableNoTraking
                  .SelectMany(g => _unitOfWork.GetRepository<IssueUser>().TableNoTraking.Where(e => e.IssueId == g.Id).DefaultIfEmpty(),
                        (issue, issueUser) => new { issue,issueUser })
                  .SelectMany(eu => all.ToList().Where(e => e.Id == eu.issueUser.UserId).DefaultIfEmpty(),
                        (ms, u) => new { ms.issue,ms.issueUser,u})
                  .Join(_unitOfWork.GetRepository<Project>().Table,
                      ms=>ms.issue.ProjectId,
                      p=>p.Id,
                      (ms, p) => new{ms.issue,ms.issueUser,ms.u,p })
                  .GroupBy(e=>e.issue.Id)
                  .Select(e=>new
                  {
                      Issue=e.Key,
                      Name=e.First().issue.Title,
                      UserName = e.First().u==null?"": e.First().u.Email
                  }).ToList();



            var issueList=await query.ToListAsync();
            foreach (var item in issueList)
            {
                foreach (var eu in item.IssueUser)
                {
                    var user = await _userManager.FindByIdAsync(eu.UserId).ConfigureAwait(false);
                }
            }
            return _mapper.Map<IEnumerable<IssueResp>>(issueList);
        }


        public async Task<Result<IssueResp>> AddAsync(AddIssueRqst rqst)
        {
            var entity=_mapper.Map<Issue>(rqst);
            var addedResult=await _repository.AddAsync(entity).ConfigureAwait(false);
            await _unitOfWork.CommitAsync().ConfigureAwait(false);
            return await Result<IssueResp>.SuccessAsync(_mapper.Map<IssueResp>(addedResult), 
                                                    _commonLocalizer["Added"].FormatWith("Issue", rqst.Title)).ConfigureAwait(false);
        }

        public async Task<Result<Issue>> FindByIdAsync(int id)
        {
            var issue=await _repository.GetAsync(id).ConfigureAwait(false);
            if (issue == null)
                return await Result<Issue>.FailAsync(_commonLocalizer["NotFound"]).ConfigureAwait(false);
            return await Result<Issue>.SuccessAsync(issue).ConfigureAwait(false);
        }

        public async Task<Result<IssueResp>> UpdateAsync(Issue issue)
        {
            var updatedIssue= _repository.Update(issue);
            await _unitOfWork.CommitAsync().ConfigureAwait(false);
            return await Result<IssueResp>.SuccessAsync(_mapper.Map<IssueResp>(updatedIssue)).ConfigureAwait(false);

        }

        public async Task<Result<IssueResp>> DeleteAsync(Issue issue)
        {
            _repository.Delete(issue);
            await _unitOfWork.CommitAsync().ConfigureAwait(false);
            return await Result<IssueResp>.SuccessAsync(_commonLocalizer["Deleted"].FormatWith("Issue"))
                                          .ConfigureAwait(false);
        }
        #endregion
    }
}
