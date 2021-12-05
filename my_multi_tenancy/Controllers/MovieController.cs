using DeviceManager.Api.Data.Management;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using my_multi_tenancy.Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace my_multi_tenancy.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MovieController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<MovieController> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public MovieController(ILogger<MovieController> logger,IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Movie>> Get()
        {
            return Ok(_unitOfWork.GetRepository<Movie>().GetAll().ToList());
        }
    }
}
