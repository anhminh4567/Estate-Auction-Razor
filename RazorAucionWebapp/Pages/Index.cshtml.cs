using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorAucionWebapp.Pages.AdminPages.Accounts;
using Repository.Database.Model;
using Repository.Database.Model.AuctionRelated;
using Repository.Database.Model.RealEstate;
using Service.MyHub;
using Service.MyHub.HubServices;
using Service.Services.AppAccount;
using Service.Services.Auction;
using Service.Services.RealEstate;
using System.Security.Claims;

namespace RazorAucionWebapp.Pages
{
    //public class Item
    //{
    //    public Auction Auction { get; set; }
    //    public bool isJoined { get; set; }
    //}
    public class IndexModel : PageModel
    {
        private readonly AuctionServices _auctionServices;
		private readonly EstateImagesServices _estateImagesServices;
		public IndexModel(AuctionServices auctionServices, EstateImagesServices estateImagesServices)
        {
            _auctionServices = auctionServices;
			_estateImagesServices = estateImagesServices;
		}
        public List<Auction> Auctions { get; set; } = new List<Auction>();
        [BindProperty(SupportsGet = true)]
        public int PageStart { get; set; } = 1;
        [BindProperty]
        public int DisplayAmount { get; set; } = 6;
        [BindProperty]
        public int TotalPages { get; set; }
		public async Task<IActionResult> OnGetAsync()
        {
            int correctStartValue = (PageStart-1) * DisplayAmount;
            await PopulateData(correctStartValue, DisplayAmount);

			foreach (var auction in Auctions)
			{
				var appImages = await _estateImagesServices.GetByEstateId(auction.Estate.EstateId); // this will return list of image
				foreach (var appImage in appImages)
				{
					var image = auction.Estate.Images.FirstOrDefault(i => i.Image.Path == appImage.Path);
					if (image != null)
					{
						image.Image.Path = "~/PublicImages/storage/" + appImage.Path;
					}
				}
			}
			return Page();
        }
        private async Task PopulateData(int start, int amount ) 
        {

            var list = await _auctionServices.GetRangeInclude_Estate_Company(start,amount);
            Auctions = list;
            List<Auction> aucList = await _auctionServices.GetAll();
            TotalPages = (int) Math.Ceiling(decimal.Divide(aucList.Count, amount));
            //Auctions = new List<Auction>();
            //foreach(var i in list)
            //{
            //    Auctions.Add(new Item()
            //    {
            //        Auction = i,
            //        isJoined = await _auctionServices.CheckForJoinedAuction(UserId, i.AuctionId)
            //    });
            //}
        }
	}
}