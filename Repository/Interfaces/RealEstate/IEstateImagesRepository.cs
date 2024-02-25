using Repository.Database.Model.RealEstate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Interfaces.RealEstate
{
	public interface IEstateImagesRepository : ICrud<EstateImages>
	{
		Task<List<EstateImages>?> GetByEstateId(int estateId);
	}
}
