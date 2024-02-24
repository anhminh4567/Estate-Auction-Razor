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

		public async Task<Estate?> GetByCompanyId(int companyId)
		{
			return await _set.FirstOrDefaultAsync(e => e.CompanyId == companyId);
		}

		public async Task<Estate?> GetFullAsync(int id)
		{
			return await _set.Include(e => e.Company).Include(e => e.Auctions).FirstOrDefaultAsync(e => e.EstateId == id);
		}
	}
}
