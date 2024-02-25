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
	public class EstateCategoriesRepository : BaseRepository<EstateCategories>, IEstateCategoriesRepository
	{
		public EstateCategoriesRepository(AuctionRealEstateDbContext context) : base(context)
		{
		}

		public async Task<List<EstateCategories>> GetByCategoryId(int categoryId)
		{
			return await _set.Where(e => e.CategoryId == categoryId).ToListAsync();
		}

		public async Task<List<EstateCategories>> GetByEstateId(int estateId)
		{
			return await _set.Where(e => e.EstateId == estateId).ToListAsync();
		}
	}
}
