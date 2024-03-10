using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore.Metadata;
using Newtonsoft.Json;
using Repository.Database.Model.AuctionRelated;
using Repository.Database.Model.RealEstate;
using Repository.Interfaces.DbTransaction;

namespace Service.MyHub.HubServices
{
	public class AuctionHubService
	{
		private readonly IHubContext<AuctionHub> _auctionHub;
		private readonly IUnitOfWork _unitOfWork;

		public AuctionHubService(IHubContext<AuctionHub> auctionHub, IUnitOfWork unitOfWork)
		{
			_auctionHub = auctionHub;
			_unitOfWork = unitOfWork;
		}

		public async Task CreateAuctionSuccess(Auction auction)
		{
			var estate = await _unitOfWork.Repositories.estateRepository.GetAsync(auction.EstateId);
			var estateCategories = (await _unitOfWork.Repositories.estateCategoriesRepository
				.GetByCondition(e => e.EstateId == auction.EstateId, includeProperties: "CategoryDetail"))
				.Select(e => e.CategoryDetail);
			var dayremain = (auction.EndDate - auction.StartDate).Days;
			var area = estate.Width * estate.Length;
			var bids = (await _unitOfWork.Repositories.bidRepository.GetByCondition(b => b.AuctionId == auction.AuctionId)).OrderByDescending(b => b.Amount);
			Bid highestbid = new Bid() { Amount = 0 };
			if (bids.Count() != 0)
				highestbid = bids.First();
			await _auctionHub.Clients.Group("all").SendAsync("CreatedObject", auction, estate, estateCategories, dayremain, area, highestbid);
		}
		public Task DeleteAuctionSuccess(Auction auction)
		{
			return Task.CompletedTask;
		}
		public async Task UpdateAuctionSuccess(Auction auction)
		{
			//var json = JsonConvert.SerializeObject(auction,new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore});
			await _auctionHub.Clients.Group("all").SendAsync("UpdatedObject", auction);
		}
	}
}
