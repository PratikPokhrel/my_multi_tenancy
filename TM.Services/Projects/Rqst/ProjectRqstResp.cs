using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TM.Core.Enums;

namespace TM.Services.Projects.Rqst
{
    public class ProjectRqstResp
    {
        [Required]
        public string Name { get; set; }
        public string Url { get; set; }

        [Required]
        public string Description { get; set; }
        public ProjectCategory Category { get; set; }
    }
}
