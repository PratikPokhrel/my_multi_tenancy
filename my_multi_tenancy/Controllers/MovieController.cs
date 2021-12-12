using Core.EF.Data.Context;
using Core.Entities;
using Core.Infrastructure.DataAccess;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using my_multi_tenancy.FIlters;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace my_multi_tenancy.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [IsUserInTenant]
    public class MovieController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<MovieController> _logger;
        private readonly IDefaultUnitOfWork _unitOfWork;
        private readonly IUnitOfWork _uow;
        private readonly IHttpContextAccessor _accessor;

        public MovieController(ILogger<MovieController> logger, IDefaultUnitOfWork unitOfWork, IUnitOfWork uow, IHttpContextAccessor accessor)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _uow = uow;
            _accessor = accessor;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Menu>>> Get()
        {
            return Ok(await _uow.GetRepository<Menu>().GetAllAsync().ConfigureAwait(false));
        }

        [HttpPost]
        public ActionResult<int> Post([FromBody] Movie movie)
        {
            var res = _unitOfWork.GetRepository<Movie>().Add(movie);
            _unitOfWork.Commit();
            return Ok(res);
        }


        // 5249f843-d4a3-4d9c-b0ff-bc1a9d3cd5e1
        private void HasDuplicate()
        {
            var anyyy = _unitOfWork.GetRepository<Movie>().TableNoTraking
                                                          .GroupBy(e => e.Name)
                                                          .Select(e => new { e.Key, Count = e.Count() })
                                                          .Where(j => j.Count > 0);
            var ex = anyyy.Where(e => e.Count > 1);
            var query = ex.ToQueryString();
            var listed = ex.ToList();
        }

       

    }
}
