using Core.Entities;
using my_multi_tenancy.Controllers.Branches.Dtos.Resp;

namespace my_multi_tenancy.Controllers.Branches.Dtos
{
    internal static class BranchMapper
    {
        public static Branch MapToEntity(this BranchRqst branchRqst, Branch branch = null)
        {
            if (branchRqst == null)
                return null;

            if (branch == null)
                return new Branch
                {
                    Name = branchRqst.Name,
                    IsDefault = branchRqst.IsDefault,
                };

            branch.Name = branchRqst.Name;
            branch.IsDefault = branchRqst.IsDefault;
            return branch;
        }
    }
}
