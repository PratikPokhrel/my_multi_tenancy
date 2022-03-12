using Core.Entities;
using Microsoft.AspNetCore.Mvc;
using Services.Services;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace my_multi_tenancy.Controllers
{
    /// <summary>
    /// Tenant Controller
    /// </summary>
    [Route("api/tenants")]
    public class TenantController : Controller
    {
        private readonly ITenantService _tenantService;
        private readonly IMovieService _movieService;

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="tenantService"></param>
        /// <param name="movieService"></param>
        public TenantController(ITenantService tenantService, IMovieService movieService)
        {
            _tenantService = tenantService;
            _movieService = movieService;
        }

        /// <summary>
        /// Get all tenants
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Tenant>>> GetAll()
        {
            var tenants = await _tenantService.GetAllAsync().ConfigureAwait(false);
            var movies =  _movieService.GetAll();
            return Ok(new{tenants,movies});
        }
    }
}
