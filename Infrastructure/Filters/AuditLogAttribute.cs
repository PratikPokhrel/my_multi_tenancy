//using Microsoft.AspNetCore.Mvc.Filters;
//using Microsoft.Extensions.Logging;
//using System;
//using System.Collections.Generic;
//using System.Diagnostics;
//using System.Linq;
//using System.Security.Claims;
//using System.Text;
//using System.Threading.Tasks;

//namespace Infrastructure.Filters
//{
//    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
//    public class AuditLogAttribute : ActionFilterAttribute
//    {
//        /// <summary>
//        /// Logger instance
//        /// </summary>
//        private ILogger _logger;
//        private IActivityLogService _activityLogService;
//        private Stopwatch stopwatch;

//        /// <summary>
//        /// activity log 
//        /// </summary>
//        private readonly ActivityLog activitylog;

//        /// <summary>
//        /// Log activity
//        /// </summary>
//        public bool LogActivity { get; set; }

//        /// <summary>
//        /// Ctor
//        /// </summary>
//        /// <param name="LogActivity"></param>
//        public AuditLogAttribute(bool LogActivity = false)
//        {
//            activitylog = new ActivityLog { Type = ActivityLogType.General };
//        }

//        /// <summary>
//        /// Save log before action execution
//        /// </summary>
//        /// <param name="context"></param>
//        public override void OnActionExecuted(ActionExecutedContext context)
//        {
//            stopwatch.Stop();
//            activitylog.ExecutionDuration = (int)stopwatch.Elapsed.TotalMilliseconds;

//            if (LogActivity && !string.IsNullOrEmpty(activitylog.UserName))
//            {
//                activitylog.Comment = activitylog.ToString();
//                _activityLogService.CreateAsync(activitylog).GetAwaiter().GetResult();
//            }
//            _logger.LogInformation(activitylog.ToString());
//        }

//        /// <summary>
//        /// Save log after action execution
//        /// </summary>
//        /// <param name="context"></param>
//        public override void OnActionExecuting(ActionExecutingContext context)
//        {
//            stopwatch = Stopwatch.StartNew();
//            _activityLogService = (IActivityLogService)context.HttpContext.RequestServices.GetService(typeof(IActivityLogService));
//            _logger = (ILogger)context.HttpContext.RequestServices.GetService(typeof(ILogger<AuditLogAttribute>));
//            if (context.HttpContext.User.Identity.IsAuthenticated)
//            {
//                var claim = context.HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);
//                activitylog.UserName = context.HttpContext.User.Identity.Name;
//                activitylog.UserId = claim?.Value;
//            }

//            activitylog.ServiceName = $"{context.HttpContext.Request.Method} {context.HttpContext.Request.Path}";
//            _activityLogService.FillClientInfo(activitylog);
//            _logger.LogInformation($"AUDIT LOG: {activitylog.ServiceName } is requested by tenant/user {activitylog.TenantId}/{activitylog.UserId}  from { activitylog.ClientIpAddress} IP address.");
//        }
//    }
//}
