using Repository.Database.Model.AuctionRelated;
using Repository.Interfaces.Auction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services.AuctionService
{
	public  class AuctionReceiptServices
	{
		private readonly IAuctionReceiptRepository _auctionReceiptRepository;

		public AuctionReceiptServices(IAuctionReceiptRepository auctionReceiptRepository)
		{
			_auctionReceiptRepository = auctionReceiptRepository;
		}

		public async Task<AuctionReceipt> GetAuctionReceipt(int id)
		{
			return await _auctionReceiptRepository.GetAsync(id);
		}
		public async Task<List<AuctionReceipt>> GetAllAuctionReceipt()
		{
			return await _auctionReceiptRepository.GetAllAsync();
		}
		public async Task<AuctionReceipt> CreateAuctionReceipt(AuctionReceipt auctionReceipt)
		{
			return await _auctionReceiptRepository.CreateAsync(auctionReceipt);
		}
		public async Task<bool> UpdateAuctionReceipt(AuctionReceipt auctionReceipt)
		{
			return await _auctionReceiptRepository.UpdateAsync(auctionReceipt);
		}
		public async Task<bool> DeleteAuctionReceipt(AuctionReceipt auctionReceipt)
		{
			return await _auctionReceiptRepository.DeleteAsync(auctionReceipt);
		}
	}
}
