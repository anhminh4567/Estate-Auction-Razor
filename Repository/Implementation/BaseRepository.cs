using Microsoft.EntityFrameworkCore;
using Repository.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Implementation
{
	public class BaseRepository<T> where T : class
	{
		private readonly AuctionRealEstateDbContext _context;
		protected readonly DbSet<T> _set;
		public BaseRepository(AuctionRealEstateDbContext context)
		{
			_context = context;
			_set = _context.Set<T>();
		}
		public virtual async Task<List<T>> GetAllAsync()
		{
			return await _set.ToListAsync();
		}

		public virtual async Task<T> GetAsync(int id)
		{
			return await _set.FindAsync(id);
		}
        public Task<List<T>> GetByCondition(Expression<Func<T, bool>> expression = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            string includeProperties = "")
		{
            IQueryable<T> query = _context.Set<T>();
            if (expression != null)
            {
                query = query.Where(expression);
            }

            foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            if (orderBy != null)
            {
                return orderBy(query).ToListAsync();
            }
            else
            {
                return query.ToListAsync();
            }
        }
        public virtual async Task<T> CreateAsync(T t)
		{
			var obj = await _set.AddAsync(t);
			await _context.SaveChangesAsync();
			return obj.Entity;
		}

        public virtual async Task<bool> DeleteAsync(T t)
		{
			if (t == null)
			{
				return false;
			}
			_set.Remove(t);
			await _context.SaveChangesAsync();
			return true;
		}
		public virtual async Task<bool> DeleteRange(List<T> ts) 
		{
			if (ts is null) 
			{
				return false;
			}
			_set.RemoveRange(ts);
			await _context.SaveChangesAsync();
			return true;
		}
		public virtual async Task<bool> UpdateAsync(T t)
		{
			if (t == null)
			{
				return false;
			}
			_set.Update(t);
			await _context.SaveChangesAsync();
			return true;
		}
	}
}
