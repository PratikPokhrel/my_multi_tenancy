using Core.Infrastructure.DataAccess;
using Microsoft.AspNetCore.Mvc;
using Core.Entities;
using Core.Dto;
using Core.Infrastructure;
using Core.Entities.QueryModels;
using Microsoft.EntityFrameworkCore;

namespace my_multi_tenancy.Controllers
{
    [Route("api/categories")]
    public class CategoryController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        #region Ctor & properties
        public CategoryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Core.Entities.Category>>> GetAllAsync()
        {
            ThisIsMyFunction();
            var qq = _unitOfWork.GetRepository<CategoryItemQuery>().FromSql(@"select c.id as categoryid,c.name as categoryname,ci.name as categoryitemname 
                                                                            from category c
                                                                            left join category_item ci on c.id = ci.category_id");
            var queryable=qq.Where(e=>e.CategoryId==39);
            var mn=queryable.ToArray();

            return Ok(await _unitOfWork.GetRepository<Core.Entities.Category>().GetAllAsync().ConfigureAwait(false));

        }

        [HttpPost]
        public async Task<ActionResult<Result<Core.Entities.Category>>> AddAsync([FromBody] Core.Entities.Category category)
        {
            var added = _unitOfWork.GetRepository<Core.Entities.Category>().Add(category);
            await _unitOfWork.CommitAsync().ConfigureAwait(false);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Core.Entities.Category>> DeleteAsync(int id)
        {
            var added = await _unitOfWork.GetRepository<Core.Entities.Category>().GetAsync(e => e.Id == id);
            var del = _unitOfWork.GetRepository<Core.Entities.Category>().Delete(added);
            await _unitOfWork.CommitAsync().ConfigureAwait(false);
            return Ok(added);
        }

        [NonAction]
        public List<string> ThisIsMyFunction()
        {
            var lstring=new List<string>() {};
            if(!lstring.Any()) return new List<string>();
            {
                return lstring;
            }
        }
        #endregion

    }
}
