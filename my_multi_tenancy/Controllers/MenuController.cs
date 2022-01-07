using Core.Dto;
using Core.Entities;
using Microsoft.AspNetCore.Mvc;
using my_multi_tenancy.FIlters;
using Services.Features.Menus.Queries;
using Services.Services;
using Services.Services.Dtos.Resp;
using Services.Services.Dtos.Rqst;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using Infrastructure.Filters;

namespace my_multi_tenancy.Controllers
{
    [Route("api/menus")]
    public class MenuController : BaseApiController
    {
        private readonly IMenuService _menuService;

        public MenuController(IMenuService menuService)
        {
            _menuService = menuService;
        }

        [HttpGet]
        public async Task<ActionResult<Result<List<MenuResp>>>> GetListAsync()
        {
            var response = await _menuService.GetMenusAsync().ConfigureAwait(false);
                //await _mediator.Send(new GetAllMenuQuery());
            return AppOk(response);
        }

        [HttpPost("change-language")]
        public ActionResult ChangeLanguage()
        {
            var culture = new CultureInfo("en-US");
            CultureInfo.DefaultThreadCurrentCulture = culture;
            CultureInfo.DefaultThreadCurrentUICulture = culture;
            return Ok();
        }

        [ModelStateValidator]
        [HttpPost]
        public async Task<ActionResult<Result<MenuResp>>> AddAsync([FromBody]MenuRqst rqst)
        {
            var resp = await _menuService.AddAsync(rqst.ToEntity()).ConfigureAwait(false);
            if(!resp.Succeeded)
                return AppOk(resp);

            rqst.Id=resp.Data.Id;
            return AppOk(resp,rqst);
        }
    }
}
