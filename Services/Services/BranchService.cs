using Core.Constants;
using Core.Dto;
using Core.Entities;
using Core.Infrastructure.DataAccess;
using Core.Infrastructure.Pagination;
using LazyCache;
using Services.Services.Branches;
using Services.Services.Branches.Resp;
using System.Linq.Expressions;
using Microsoft.Extensions.Localization;
using Core;
using Core.Infrastructure;
using AutoMapper;

namespace Services.Services
{
    public class BranchService : IBranchService
    {
        #region "Ctor & Properties"

        private readonly IRepository<Branch> _repository;
        private readonly IAppCache _cache;
        private readonly IBranchUserService _branchUserService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPager _pager;
        private readonly IStringLocalizer<BranchService> _localizer;
        private readonly IStringLocalizer _commonLocalizer;
        private readonly IMapper _mapper;
        private readonly string serviceName = "Branch";

        public BranchService(IUnitOfWork unitOfWork, IAppCache cache, IBranchUserService branchUserService,
            IPager pager, IStringLocalizer<BranchService> localizer, ICommonLocalizer commonLocalizer, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _repository = unitOfWork.GetRepository<Branch>();
            _cache = cache;
            _branchUserService = branchUserService;
            _pager = pager;
            _localizer = localizer;
            _mapper=mapper;
            _commonLocalizer = commonLocalizer.Localize;
        }

        #endregion

        #region "Methods"

       
        public async Task<PaginatedResult<Branch>> GetAllAsync(string name, int page, int pageSize)
        {
            Expression<Func<Branch, bool>> expression = e =>
                string.IsNullOrEmpty(name) || e.Name.ToLower().Contains(name.ToLower());
            IQueryable<Branch> branches = _repository.GetAll().Where(expression);
            return await _pager.ConvertToPagedListAsync(branches, page, pageSize).ConfigureAwait(false);
        }

        public async Task<IEnumerable<Branch>> GetAllByIds(List<Guid> ids)
        {
            return await _repository.GetAllAsync(filter: e => ids.Contains(e.Id));
        }

     
        public async Task<Result<Branch>> AddAsync(Branch branch)
        {
            var validate = await ValidateAddEditCommandAsync(branch).ConfigureAwait(false);
            if (!validate.Succeeded)
                return validate;

            Branch added = await _repository.AddAsync(branch).ConfigureAwait(false);
            await _unitOfWork.CommitAndRemoveCacheAsync(CacheKeys.GET_BRANCH_DROP_DOWN).ConfigureAwait(false);
            return await Result<Branch>.SuccessAsync(added,_commonLocalizer["Added"].FormatWith(serviceName,branch.Name));
        }

       
        public async Task<Result<Branch>> UpdateAsync(Branch branch)
        {
            var validate = await ValidateAddEditCommandAsync(branch).ConfigureAwait(false);
            if (!validate.Succeeded)
                return validate;

            Branch updated = _repository.Update(branch);
            await _unitOfWork.CommitAndRemoveCacheAsync(CacheKeys.GET_BRANCH_DROP_DOWN).ConfigureAwait(false);
            return await Result<Branch>.SuccessAsync(updated,_localizer["Updated"].FormatWith(serviceName,branch.Name));
        }


      
        public async Task<Result<Branch>> ValidateAddEditCommandAsync(Branch branch)
        {
            if (await _repository.ExistsAsync(e => e.Name.ToLower().Equals(branch.Name.ToLower())))
                return await Result<Branch>.FailAsync(_commonLocalizer["AlreadyExists"].FormatWith(branch.Name));

            return await Result<Branch>.SuccessAsync();
        }

      
        public async Task<Result<BranchServiceResp>> GetAllById(Guid id)
        {
            Branch branch = await _repository.GetAsync(e => e.Id == id).ConfigureAwait(false);
            if (branch == null)
                return await Result<BranchServiceResp>.FailAsync(_commonLocalizer["NotFound"]);

            var branchDto = _mapper.Map<BranchServiceResp>(branch);
            branchDto.Users = await _branchUserService.GetBranchUsers(branch.Id).ConfigureAwait(false);
            return await Result<BranchServiceResp>.SuccessAsync(data: branchDto);
        }


     
        public async Task<Result<Branch>> DeleteAsync(Guid id)
        {
            var branch = await _repository.GetAsync(e => e.Id == id).ConfigureAwait(false);
            _repository.Delete(branch);
            await _unitOfWork.CommitAndRemoveCacheAsync(CacheKeys.GET_BRANCH_DROP_DOWN).ConfigureAwait(false);
            return await Result<Branch>.SuccessAsync(_commonLocalizer["Deleted"].FormatWith(serviceName));
        }


        public async Task<IEnumerable<DropDownDto<Guid>>> GetDropDownAsync(string search)
        {
            async Task<IEnumerable<DropDownDto<Guid>>> getBranchDropDown()
            {
                return await _repository.GetAllAsync(where: e => string.IsNullOrEmpty(search) ||
                                                                 e.Name.ToLower().Contains(search.ToLower()),
                        select: b => new DropDownDto<Guid>() {Text = b.Name, Value = b.Id},
                        orderBy: b => b.OrderByDescending(bb => bb.Name))
                    .ConfigureAwait(false);
            }

            return await _cache.GetOrAddAsync(CacheKeys.GET_BRANCH_DROP_DOWN, getBranchDropDown);
        }

        #endregion
    }
}