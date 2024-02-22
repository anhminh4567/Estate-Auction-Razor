using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Interfaces
{
	public interface ICrud<T> where T : class
	{
		Task<T> GetAsync(int id);
		Task<List<T>> GetAllAsync();
		Task<T> CreateAsync(T t);
		Task<bool> UpdateAsync(T t);	
		Task<bool> DeleteAsync(T t);
	}
}
