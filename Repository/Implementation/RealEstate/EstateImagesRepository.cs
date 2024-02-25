using Microsoft.EntityFrameworkCore;
using Repository.Database;
using Repository.Database.Model.RealEstate;
using Repository.Implementation;
using Repository.Interfaces.RealEstate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Implementation.RealEstate
{
	public class EstateImagesRepository : BaseRepository<EstateImages>, IEstateImagesRepository
	{
		public EstateImagesRepository(AuctionRealEstateDbContext context) : base(context)
		{

		}

		public async Task<List<EstateImages>?> GetByEstateId(int estateId)
		{
			return await _set.Where(e => e.EstateId == estateId).ToListAsync();
		}
	}
}
