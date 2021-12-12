using Core.Entities;
using Core.Infrastructure.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services
{
    public class MovieService : IMovieService
    {

        #region Ctor & properties
        protected readonly IUnitOfWork _uow;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="uow"></param>
        public MovieService(IUnitOfWork uow)
        {
            _uow=uow;
        }
        #endregion
        public IEnumerable<Movie> GetAll()
        {
            var findById=_uow.GetRepository<Movie>().GetAsync(new Guid("5c4c82d4-bbb4-43f4-bd2f-8db4a1bfb012")).Result;
            return _uow.GetRepository<Movie>().GetAll().ToList();
        }
    }
}
