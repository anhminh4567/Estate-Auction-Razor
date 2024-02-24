using Repository.Database.Model.AuctionRelated;
using Repository.Interfaces.Auction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services.AuctionService
{
	public class BidServices
	{
		private readonly IBidRepository _bidRepository;

		public BidServices(IBidRepository bidRepository)
		{
			_bidRepository = bidRepository;
		}
		public async Task<List<Bid>> GetBidsByAccountId(int accountId) 
		{
			return await _bidRepository.GetByAccountId(accountId);
		}
	}
}
