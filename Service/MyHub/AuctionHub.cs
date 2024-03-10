using Microsoft.AspNetCore.SignalR;
using Repository.Database.Model.AuctionRelated;
using Repository.Database.Model.RealEstate;
using Repository.Interfaces.DbTransaction;
using System.Runtime.InteropServices;

namespace Service.MyHub
{
	public class AuctionHub : Hub
	{
		private readonly IUnitOfWork _unitOfWork;

		public AuctionHub(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public override async Task OnConnectedAsync()
		{
			var context = Context.GetHttpContext();
			// use identity to set to group if necesesary
			//await Clients.Caller.SendAsync("ReceiveMessage", Context.ConnectionId.ToString());
			await Groups.AddToGroupAsync(connectionId: Context.ConnectionId, "all");
		}
		public override async Task OnDisconnectedAsync(Exception? exception)
		{
			await Groups.RemoveFromGroupAsync(connectionId: Context.ConnectionId, "all");
		}

		internal async Task CreateAuction(Auction auction)
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
			await Clients.All.SendAsync("CreatedObject", auction, estate, estateCategories, dayremain, area, highestbid);
		}
		internal async Task DeleteAuction(Auction auction)
		{
			await Clients.All.SendAsync("DeletedObject", auction);
		}
		internal async Task UpdateAuction(Auction auction)
		{
			await Clients.All.SendAsync("UpdatedObject", auction);
		}
	}
}
