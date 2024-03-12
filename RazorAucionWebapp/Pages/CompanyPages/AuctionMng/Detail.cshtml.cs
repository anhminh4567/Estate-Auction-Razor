using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Repository.Database;
using Repository.Database.Model.AuctionRelated;
using Service.MyHub.HubServices;
using Service.Services.AppAccount;
using Service.Services.Auction;
using Service.Services.RealEstate;

namespace RazorAucionWebapp.Pages.CompanyPages.AuctionMng
{
    public class DetailModel : PageModel
    {
        private readonly AuctionServices _auctionService;
        private readonly EstateServices _estateService;
        private readonly BidServices _bidService;
        private readonly AuctionReceiptServices _auctionReceiptService;
        private readonly JoinedAuctionServices _joinedAuctionService;
        private readonly AuctionHubService _auctionHubService;
        private readonly EstateImagesServices _estateImagesServices;


        public DetailModel(AuctionServices auctionService, EstateServices estateService, BidServices bidService, 
            AuctionReceiptServices auctionReceiptService, JoinedAuctionServices joinedAuctionService, 
            AuctionHubService auctionHubService, EstateImagesServices estateImagesServices)
		{
			_auctionService = auctionService;
			_estateService = estateService;
			_bidService = bidService;
			_auctionReceiptService = auctionReceiptService;
			_joinedAuctionService = joinedAuctionService;
			_auctionHubService = auctionHubService;
            _estateImagesServices = estateImagesServices;
		}

		public Auction Auction { get; set; } = default!;
        public List<string> Images { get; set; } = new List<string>();

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            try {
                await PopulateData(id.Value);
            }catch (Exception ex) { 
                Console.WriteLine(ex.Message);
                //ModelState.AddModelError(string.Empty, ex.Message);
                return NotFound();
            }
            var getAuctionDetail = await _auctionService.GetInclude(id.Value, "Estate,Bids,AuctionReceipt.Buyer,JoinedAccounts.Account");
            if (getAuctionDetail is null) 
            {
                return NotFound();
            }
            Auction = getAuctionDetail;
            await GetImages();
            //await _auctionHubService.UpdateAuctionSuccess(Auction);
            //await _auctionHubService.CreateAuctionSuccess(Auction);
            return Page();
        }
        private async Task PopulateData(int id) 
        {
            var getAuctonDetail = await _auctionService.GetInclude(id, "Bids,JoinedAccounts,Estate");
            if (getAuctonDetail is null)
                throw new Exception("cant find Auctoin with such id");
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
