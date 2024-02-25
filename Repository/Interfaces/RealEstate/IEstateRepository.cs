using Repository.Database.Model.RealEstate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Interfaces.RealEstate
{
	public interface IEstateRepository : ICrud<Estate>
	{
		Task<Estate> GetFullAsync(int id);
		Task<List<Estate>> GetByCompanyId(int id);
	}
}
