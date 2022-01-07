using System;
using Core.Dto;
using Core.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Services.Services;
using Microsoft.AspNetCore.Http;

namespace my_multi_tenancy.Controllers
{
    [ApiController]
    //[IsUserInTenant]
    public abstract class BaseApiController : ControllerBase
    {
        private IMediator _mediatorInstance;
        protected IMediator _mediator => _mediatorInstance ??= HttpContext.RequestServices.GetService<IMediator>();
        private IResponseLogService _responseLogServiceInstance;
        private IResponseLogService _responseLogService =>_responseLogServiceInstance??= HttpContext.RequestServices.GetService<IResponseLogService>();
        private IActionContextAccessor _actionContextAccessorInstance;
        protected IActionContextAccessor _actionContextAccessor=>_actionContextAccessorInstance??=HttpContext.RequestServices.GetService<IActionContextAccessor>();


        private IHttpContextAccessor _httpContextAccessorInstance;
        protected IHttpContextAccessor _httpContextAccessor => _httpContextAccessorInstance ??= HttpContext.RequestServices.GetService<IHttpContextAccessor>();


        protected ObjectResult AppOk(object obj)
        {
            var retObj = new Result<object>(true, obj, "Data retrieved successfully".ToStringList(),MessageTypes.Success);
            AddLog(retObj);
            return Ok(retObj);
        }

        protected OkObjectResult AppOk<T>(T obj) where T : Core.Dto.IResult
        {
            AddLog(obj);
            return Ok(obj);
        }

        protected OkObjectResult AppOk<T>(T obj, object dto) where T : Core.Dto.IResult
        {
            var returnObj = new Result<object>(obj.Succeeded, obj.Messages,obj.MessageType)
            {
                Data = dto
            };
            AddLog(returnObj);
            return Ok(returnObj);
        }

        [NonAction]
        public void AddLog(object obj)
        {
            try
            {
                var ari = ControllerContext.ActionDescriptor.AttributeRouteInfo;
                var apiName = ari.Name ?? ari.Template;

                var rd = _actionContextAccessor.ActionContext.RouteData;
                string currentController = rd.Values["controller"].ToString();
                string currentAction = rd.Values["action"].ToString();
                _responseLogService.AddAsync(obj, currentController + "/" + currentAction,apiName).GetAwaiter().GetResult();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}