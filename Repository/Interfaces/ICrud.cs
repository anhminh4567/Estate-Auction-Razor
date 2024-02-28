using Repository.Database.Model.RealEstate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Interfaces
{
	public interface ICrud<T> where T : class
	{
		Task<T> GetAsync(int id);
		Task<List<T>> GetAllAsync();
        Task <List<T>> GetByCondition(Expression<Func<T, bool>> expression = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            string includeProperties = "");
        Task<T> CreateAsync(T t);
		Task<bool> UpdateAsync(T t);	
		Task<bool> DeleteAsync(T t);
        Task<bool> DeleteRange(List<T> ts);
    }
}
