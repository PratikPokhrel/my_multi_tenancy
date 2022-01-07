using System;
using System.ComponentModel.DataAnnotations;

namespace my_multi_tenancy.Controllers.Branches.Dtos
{
    public class BranchDto
    {
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; }
        public bool IsDefault { get; set; }
    }
}
