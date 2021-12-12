using Core.Entities;
using Microsoft.AspNetCore.Mvc;
using Services.Services;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace my_multi_tenancy.Controllers
{
    [Route("api/tenants")]
    public class TenantController : Controller
    {
        private readonly ITenantService _tenantService;
        private readonly IMovieService _movieService;

        public TenantController(ITenantService tenantService, IMovieService movieService)
        {
            _tenantService = tenantService;
            _movieService = movieService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Tenant>>> GetAll()
        {
            var tenants = await _tenantService.GetAllAsync().ConfigureAwait(false);
            var movies =  _movieService.GetAll();
            return Ok(new{tenants,movies});
        }
    }
}
