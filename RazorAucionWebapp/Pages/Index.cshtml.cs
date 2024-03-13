using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
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
        //frontend lo 2 so nay, dung paging
        [BindProperty]
        public int PageStart { get; set; }
        [BindProperty]
        public int DisplayAmount { get; set; }
		// PageStart: la vi tri bat dau trong database
		// DisplayAmount: la lay bao nhieu tu vi tri do
		public async Task<IActionResult> OnGetAsync()
        {
            // 2 so nay la static, mot frontend sua lai, bind 2 so nay 
            PageStart = 0;
            DisplayAmount = 30;
            // 2 so nay la static, mot frontend sua lai, bind 2 so nay 
            int correctStartValue = PageStart * DisplayAmount;
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