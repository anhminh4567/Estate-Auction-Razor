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
		public async Task<List<Database.Model.AuctionRelated.Auction>> GetActiveAuctions()
		{
			return await _set.Include(a => a.Estate).ThenInclude(p => p.Company).Where(a => a.Status == Database.Model.Enum.AuctionStatus.NOT_STARTED && a.Status == Database.Model.Enum.AuctionStatus.ONGOING).ToListAsync();
		}
		public async Task<List<Database.Model.AuctionRelated.Auction>> GetByEstateId(int estateId)
		{
			return await _set.Where(a => a.EstateId == estateId).ToListAsync();
		}
		public async Task<Database.Model.AuctionRelated.Auction?> GetFullAsync(int id)
		{
			return await _set.Include(c => c.Estate).FirstOrDefaultAsync(c => c.AuctionId == id);
		}

        public async Task<List<Database.Model.AuctionRelated.Auction>> GetRange(int start, int amount)
        {
            return await _set.Skip(start).Take(amount).ToListAsync();
        }

        public async Task<List<Database.Model.AuctionRelated.Auction>> GetRange_IncludeEstate_Company(int start, int amount)
        {
            return await _set.Include(a => a.Estate).ThenInclude(e => e.Company).Include(a => a.Estate)
					.ThenInclude(e => e.Images)
					.ThenInclude(img => img.Image).
					Include(a => a.JoinedAccounts).Skip(start).Take(amount).ToListAsync();
        }
    }
}
