using Microsoft.EntityFrameworkCore;
using Repository.Database;
using System;
using System.Collections.Generic;
using System.Linq;
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
		public async Task<List<T>> GetAllAsync()
		{
			return await _set.ToListAsync();
		}

		public async Task<T> GetAsync(int id)
		{
			return await _set.FindAsync(id);
		}

		public async Task<T> CreateAsync(T t)
		{
			var obj = await _set.AddAsync(t);
			await _context.SaveChangesAsync();
			return obj.Entity;
		}

		public async Task<bool> DeleteAsync(T t)
		{
			if (t == null)
			{
				return false;
			}
			_set.Remove(t);
			await _context.SaveChangesAsync();
			return true;
		}
		public async Task<bool> UpdateAsync(T t)
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
