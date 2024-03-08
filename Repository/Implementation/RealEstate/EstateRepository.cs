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
	public class EstateRepository : BaseRepository<Estate>, IEstateRepository
	{
		public EstateRepository(AuctionRealEstateDbContext context) : base(context)
		{

		}
		public async Task<List<Estate>> GetByCompanyId(int companyId)
		{
			return await _set.Where(e => e.CompanyId == companyId).Include(e => e.Auctions).ToListAsync();
		}
		public async Task<Estate> GetFullAsync(int id)
		{
			return await _set.Include(e => e.Company).Include(e => e.Auctions).FirstOrDefaultAsync(e => e.EstateId == id);
		}

        public async Task<Estate?> GetFullDetail(int id)
        {
			return await _set.Include(e => e.EstateCategory)?.ThenInclude(e => e.CategoryDetail)
				.Include(e => e.Auctions).Include(e => e.Company).FirstOrDefaultAsync(e => e.EstateId == id);
        }
		public async Task<Estate?> GetInclude(int id,params string[] includes) 
		{
			var query = _set.AsQueryable();
			foreach(var properties in includes) 
			{
                query = query.Include(properties);
			}
			return await query.FirstOrDefaultAsync(e => e.EstateId.Equals(id));
		}
        public async Task<List<Estate>> GetAllDetails()
        {
            return await _set.Include(e => e.EstateCategory)?.ThenInclude(e => e.CategoryDetail)
				.Include(e => e.Images).ThenInclude(e => e.Image).ToListAsync();
        }
    }
}
