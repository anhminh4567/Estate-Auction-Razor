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
		public async Task<List<Bid>> GetByAccountId(int accountId) 
		{
			return await _bidRepository.GetByAccountId(accountId);
		}
		public async Task<List<Bid>> GetByAuctionId(int auctionId) 
		{
			return await _bidRepository.GetByAuctionId(auctionId);
		}
		public async Task<Bid> Create(Bid newBid) 
		{
			return await _bidRepository.CreateAsync(newBid);
		}
		public async Task<bool> Update(Bid updateBid)
		{
			return await _bidRepository.UpdateAsync(updateBid);
		}
		public async Task<bool> Delete(Bid bidToDelete) 
		{
			return await _bidRepository.DeleteAsync(bidToDelete);
		}
	}
}
