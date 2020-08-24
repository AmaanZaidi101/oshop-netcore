using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Oshop.API.Helpers;

namespace Oshop.API.Data
{
    public interface IOshopRepository<T> where T: class
    {
         Task<IQueryable<T>> GetAll();
         Task<IQueryable<T>> GetByCondition(Expression<Func<T, bool>> expression);
         Task<PagedList<T>> GetWithPaging(Expression<Func<T, bool>> expression, PagingParams pagingParams);
         Task<PagedList<T>> GetAllWithPaging(PagingParams pagingParams);
         Task<T> GetFirst(Expression<Func<T, bool>> expression);
         Task<bool> SaveAll();
         void Insert(T entity);
         void Update(T entity);
         void Delete(T entity);
    }
}