using Core.Dto;
using Core.Dto.Security;
using Core.Entities;
using Core.Infrastructure;
using Core.Infrastructure.DataAccess;
using Core.Security.User;

namespace Services.Services.Branches
{
    public class BranchUserService : IBranchUserService
    {

        #region "Ctor & Properties"
        private IUnitOfWork _unitOfWork;
        private readonly IRepository<BranchUser> _repository;
        private readonly IApplictionUserManager _userManager;
        public BranchUserService(IUnitOfWork unitOfWork, IApplictionUserManager userManager)
        {
            _unitOfWork = unitOfWork;
            _repository=_unitOfWork.GetRepository<BranchUser>();
            _userManager=userManager;
        }
        #endregion


        #region "Service Methods"
        public async Task<IEnumerable<AppUser>> GetBranchUsers(Guid branchId)
        {
            List<Guid> userIds=await _repository.GetAllAsync(select:e=>e.UserId,where:e=>e.BranchId==branchId).ConfigureAwait(false);
            return await _userManager.GetManyAsync(userIds).ConfigureAwait(false);
        }

        public async Task<Result<BranchUser>> AddAsync(BranchUser user)
        {
            var addedItem= await _repository.AddAsync(user).ConfigureAwait(false);
            await _unitOfWork.CommitAsync().ConfigureAwait(false);
            return await Result<BranchUser>.SuccessAsync("Branch user added successfully");
        }


        public async Task<Result<BranchUser>> DeleteAsync(int id)
        {
            var branchUser=await _repository.GetAsync(id).ConfigureAwait(false);
            if(branchUser==null)
                return await Result<BranchUser>.FailAsync("Data not found");
            _repository.Delete(branchUser);
            await _unitOfWork.CommitAsync().ConfigureAwait(false);
            return await Result<BranchUser>.SuccessAsync("Deleted successfully");
        }

        #endregion
    }
}
