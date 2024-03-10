using Microsoft.AspNetCore.SignalR;
using Repository.Database.Model.AppAccount;
using Repository.Database.Model.AuctionRelated;
using Repository.Interfaces.DbTransaction;

namespace Service.MyHub.HubServices
{
	public class BidHubServices
	{
		private readonly IHubContext<BidHub> _bidHub;
		private readonly IUnitOfWork _unitOfWork;

		public BidHubServices(IHubContext<BidHub> bidHub, IUnitOfWork unitOfWork)
		{
			_bidHub = bidHub;
			_unitOfWork = unitOfWork;
		}

		public async Task SendNewBid(Bid newBid, Account bidder)
		{
			await _bidHub.Clients.All.SendAsync("OnNewBid", newBid, bidder);
		}
		public async Task DeleteBids(List<Bid> deletedBids)
		{
			var deletedBidsId = deletedBids.Select(b => b.BidId);
			await _bidHub.Clients.All.SendAsync("OnDeleteBids", deletedBidsId);
		}
	}
}
