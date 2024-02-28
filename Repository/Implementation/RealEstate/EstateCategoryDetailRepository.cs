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
	public class EstateCategoryDetailRepository : BaseRepository<EstateCategoryDetail>, IEstateCategoryDetailRepository
	{
		public EstateCategoryDetailRepository(AuctionRealEstateDbContext context) : base(context)
		{
		}

        public async Task<List<EstateCategoryDetail>> GetRange(params int[] ids)
        {
            return await _set.Where(e => ids.Contains(e.CategoryId)).ToListAsync();
        }
    }
}
