using Repository.Database.Model.AuctionRelated;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Interfaces.Auction
{
	public interface IBidRepository : ICrud<Bid>
	{
		Task<List<Bid>> GetByAccountId(int accountId);
		Task<List<Bid>> GetByAuctionId(int auctionId);
	}
}
