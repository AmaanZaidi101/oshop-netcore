using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Oshop.API.Helpers;

namespace Oshop.API.Data
{
    public class OshopRepository<T> : IOshopRepository<T> where T : class
    {
        private readonly DataContext _context;
        public OshopRepository(DataContext context)
        {
            _context = context;

        }
        public void Delete(T entity)
        {
            _context.Remove(entity);
        }

        public async Task<T> GetFirst(Expression<Func<T, bool>> expression)
        {
            return await _context.Set<T>().FirstOrDefaultAsync(expression);
        }

        public async Task<IQueryable<T>> GetAll()
        {
            var entities = await _context.Set<T>().ToListAsync();
            return entities.AsQueryable();
        }

        public async Task<IQueryable<T>> GetByCondition(Expression<Func<T, bool>> expression)
        {
            var entities = _context.Set<T>().AsQueryable();
            var a = await entities.Where(expression).ToListAsync();
            return a.AsQueryable();
        }

        public void Insert(T entity)
        {
            _context.Add(entity);
        }

        public void Update(T entity)
        {
            _context.Update(entity);
        }

        public async Task<bool> SaveAll()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<PagedList<T>> GetWithPaging(Expression<Func<T, bool>> expression, PagingParams pagingParams)
        {
            var entities = _context.Set<T>().AsQueryable();
            var entitiesToReturn = entities.Where(expression);
            var propertyInfo = pagingParams.OrderBy != null ? typeof(T).GetProperty(pagingParams.OrderBy):
                                typeof(T).GetProperty("Id");
            
            if(pagingParams.IsDescending)
                entitiesToReturn = entitiesToReturn.OrderByDescending(x => propertyInfo.GetValue(x, null) );
            else
                entitiesToReturn = entitiesToReturn.OrderBy(x => propertyInfo.GetValue(x, null) );

            return await PagedList<T>.CreateAsync(entitiesToReturn, pagingParams.PageNumber, pagingParams.PageSize);
        }

        public async Task<PagedList<T>> GetAllWithPaging(PagingParams pagingParams)
        {
            var entitiesToReturn = _context.Set<T>().AsQueryable();
            var propertyInfo = pagingParams.OrderBy != null ? typeof(T).GetProperty(pagingParams.OrderBy):
                                typeof(T).GetProperty("Id");
            
            if(pagingParams.IsDescending)
                entitiesToReturn = entitiesToReturn.OrderByDescending(x => propertyInfo.GetValue(x, null) );
            else
                entitiesToReturn = entitiesToReturn.OrderBy(x => propertyInfo.GetValue(x, null) );

            return await PagedList<T>.CreateAsync(entitiesToReturn, pagingParams.PageNumber, pagingParams.PageSize);
        }
    }
}