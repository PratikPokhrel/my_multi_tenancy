using System;
using System.ComponentModel.DataAnnotations.Schema;
using Core.Entities.Audit;

namespace Core.Entities
{
    public class ResponseLogs:FullAudited<Guid>,IHasTenant
    {
        [Column(TypeName = "jsonb")]
        public string Response { get; set; }
        public string TenantName { get; set; }
        public string ActionName { get; set; }
        public Guid TenantId { get; set; }
        public Guid? UserId { get; set; }
        public string ApiName { get; set; }
    }
}