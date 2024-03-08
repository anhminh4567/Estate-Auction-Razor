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
		Task<Estate> GetFullDetail(int id);
		Task<Estate?> GetInclude(int id, params string[] includes);

        Task<List<Estate>> GetByCompanyId(int id);
		Task<List<Estate>> GetAllDetails();
    }
}
