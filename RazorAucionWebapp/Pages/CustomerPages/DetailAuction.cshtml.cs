using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Repository.Database.Model.AppAccount;
using Repository.Database.Model.AuctionRelated;
using Service.Services.AppAccount;
using Service.Services.Auction;
using Service.Services.RealEstate;
using System.ComponentModel.DataAnnotations;

namespace RazorAucionWebapp.Pages.CustomerPages
{
    public class DetailAuctionModel : PageModel
    {
		private readonly BidServices _bidServices;
		private readonly AuctionServices _auctionServices;
		public DetailAuctionModel(BidServices bidServices, AuctionServices auctionServices)
		{
			_bidServices = bidServices;
			_auctionServices = auctionServices;

		}
		public Auction Auction { get; set; } = default!;
		public List<Bid> AuctionBids { get; set; }
		public List<Account>? JoinedAccounts { get; set; }
		public async Task<IActionResult> OnGetAsync(int? id)
        {
			if (id is null)
				return BadRequest();
			await PopulateData(id.Value);
			return Page();
        }
		private async Task PopulateData(int auctionId) 
		{
			var auction = await _auctionServices.GetInclude(auctionId, "Bids.Bidder,JoinedAccounts.Account,Estate.Images.Image,Estate.EstateCategory.CategoryDetail");
			if (auction == null)
				throw new Exception("no auction found for this id");
			else
				Auction = auction;
			AuctionBids = Auction.Bids.OrderByDescending(b => b.Amount).ToList();
			JoinedAccounts = Auction.JoinedAccounts
				.Select(j => j.Account)
				.ToList();
		}
    }
}
