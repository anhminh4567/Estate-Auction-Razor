using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Repository.Database.Model.AppAccount;
using Repository.Database.Model.AuctionRelated;
using Service.MyHub.HubServices;
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
        private readonly AuctionHubService _auctionHubService;
		private readonly EstateImagesServices _estateImagesServices;


		public DetailAuctionModel(BidServices bidServices, AuctionServices auctionServices
			,AuctionHubService auctionHubService, EstateImagesServices estateImagesServices)
		{
			_bidServices = bidServices;
			_auctionServices = auctionServices;
			_auctionHubService = auctionHubService;
			_estateImagesServices = estateImagesServices;
		}

		public Auction Auction { get; set; } = default!;
        public List<Bid> AuctionBids { get; set; }
        public List<Account>? JoinedAccounts { get; set; }
		public List<string> Images { get; set; } = new List<string>();
        [BindProperty]
        [Required]
        public decimal Amount { get; set; }
        [BindProperty]
        public bool isJoinedAccount { get; set; } = false;


        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id is null)
                return BadRequest();
            await PopulateData(id.Value);
            await _auctionHubService.CreateAuctionSuccess(auction: Auction);
			await GetImages();
            var tryGetId = int.TryParse(HttpContext.User.Claims.FirstOrDefault(c => c.Type.Equals("Id"))?.Value, out var bidderId);
            var getJoinedAccount = JoinedAccounts?.FirstOrDefault(a => a.AccountId == bidderId);
            if (tryGetId == true && getJoinedAccount != null)
            {
                isJoinedAccount = true;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
		{
            if (!ModelState.IsValid)
            {
                return Page();
            }
            try
            {
                return Page();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest(ex.Message);
            }
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
		private async Task GetImages()
		{
			var appImages = await _estateImagesServices.GetByEstateId(Auction.Estate.EstateId);
			foreach (var appImage in appImages)
			{
				Images.Add("~/PublicImages/storage/" + appImage.Path);
			}
		}


	}
}
