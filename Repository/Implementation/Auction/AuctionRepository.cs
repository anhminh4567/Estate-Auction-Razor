using Microsoft.EntityFrameworkCore;
using Repository.Database;
using Repository.Database.Model.AppAccount;
using Repository.Implementation;
using Repository.Interfaces.Auction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Implementation.Auction
{
	public class AuctionRepository : BaseRepository<Repository.Database.Model.AuctionRelated.Auction>, IAuctionRepository
	{
		public AuctionRepository(AuctionRealEstateDbContext context) : base(context)
		{
		}

		public async Task<List<Database.Model.AuctionRelated.Auction>> GetByEstateId(int estateId)
		{
			return await _set.Where(a => a.EstateId == estateId).ToListAsync();
		}

		public async Task<Database.Model.AuctionRelated.Auction?> GetFullAsync(int id)
		{
			return await _set.Include(c => c.Estate).FirstOrDefaultAsync(c => c.AuctionId == id);
		}
		public async Task<List<Database.Model.AuctionRelated.Auction>> GetByEstatesId(int[] estateIds) 
		{
			 throw new NotImplementedException();
		}
	}
}
