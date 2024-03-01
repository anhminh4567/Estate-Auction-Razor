using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Build.Framework;
using Microsoft.EntityFrameworkCore;
using Repository.Database;
using Repository.Database.Model.AppAccount;
using Repository.Database.Model.AuctionRelated;
using Service.Services.AppAccount;
using Service.Services.Auction;
using Service.Services.RealEstate;

namespace RazorAucionWebapp.Pages.CustomerPages
{
    public class BidAuctionModel : PageModel
    {
        private readonly BidServices _bidServices;
        private readonly AuctionServices _auctionServices;
        private readonly EstateServices _estateServices;
        private readonly JoinedAuctionServices _joinedAuctionServices;

        public BidAuctionModel(BidServices bidServices, AuctionServices auctionServices, EstateServices estateServices, JoinedAuctionServices joinedAuctionServices)
        {
            _bidServices = bidServices;
            _auctionServices = auctionServices;
            _estateServices = estateServices;
            _joinedAuctionServices = joinedAuctionServices;
        }
        public Auction Auction { get; set; } = default!;
        public List<Bid> AuctionBids { get; set; }
        public Bid? HighestBid { get; set; }
        public List<Account>? JoinedAccounts { get; set; }
        [BindProperty]
        [Required]
        public decimal Amount { get; set; }
		[BindProperty]
		[Required]
		public int AuctionID { get; set; }
		//private Bid Bid { get; set; } = default!;
		private int _bidderId;

        public async Task<IActionResult> OnGetAsync(int? auctionId)
        {
            if (auctionId is null)
                return NotFound();
            AuctionID = auctionId.Value;
            try
            {
                await PopulateData();
                return Page();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return Unauthorized();
            }

        }
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            try
            {
                await PopulateData();
                var getJoinedAccount = JoinedAccounts?.FirstOrDefault(a => a.AccountId == _bidderId);
                if (getJoinedAccount is null)
                {
                    ModelState.AddModelError(string.Empty, "user has not joined auction yet to bid");
                    return Page();
                }
                if (Amount <= HighestBid?.Amount)
                {
                    ModelState.AddModelError(string.Empty, "your cannot place a bit lower than the top dog");
                    return Page();
                }
                var compareToBidJump = (Amount - HighestBid?.Amount) >= Auction.IncrementPrice;
                if(compareToBidJump is false) 
                {
                    ModelState.AddModelError(string.Empty, "your bid must match increament price condition");
                    return Page();
                }
                var createResult = await _bidServices.Create(new Bid()
                {
                    BidderId = _bidderId,
                    AuctionId = AuctionID,
                    Amount = Amount,
                    Time = DateTime.Now,
                });
                if(createResult is null) 
                {
                    ModelState.AddModelError(string.Empty, "cannot create bid, something wrong, try again later");
                    return Page();
                }
                TempData["SuccessMessage"] = $"your bid is registered at {createResult.Time} with amount {createResult.Amount}";
                return Page();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest(ex.Message);
            }
            return RedirectToPage("./Index");
        }
        private async Task PopulateData()
        {
            var tryGetId = int.TryParse(HttpContext.User.Claims.FirstOrDefault(c => c.Type.Equals("Id"))?.Value, out var bidderId);
            if (tryGetId is false)
                throw new Exception("unauthorized user or having no required claim");
            _bidderId = bidderId;
            var auction = await _auctionServices.GetInclude(AuctionID, "Bids.Bidder,JoinedAccounts.Account,Estate.Images.Image,Estate.EstateCategory.CategoryDetail");
            if (auction == null)
                throw new Exception("no auction found for this id");
            else
                Auction = auction;
            AuctionBids = Auction.Bids.OrderByDescending(b => b.Amount).ToList();
            HighestBid = AuctionBids.FirstOrDefault();
            JoinedAccounts = Auction.JoinedAccounts
                .Select(j => j.Account)
                .ToList();
        }
    }
}
