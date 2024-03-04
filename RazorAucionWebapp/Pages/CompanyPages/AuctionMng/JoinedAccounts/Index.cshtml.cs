using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Repository.Database;
using Repository.Database.Model.AppAccount;
using Repository.Database.Model.AuctionRelated;
using Repository.Implementation.AppAccount;
using Service.Services.AppAccount;
using Service.Services.Auction;

namespace RazorAucionWebapp.Pages.CompanyPages.AuctionMng.JoinedAccounts
{
    public class IndexModel : PageModel
    {
        private readonly JoinedAuctionServices _joinedAuctionServices;
        private readonly AccountServices _accountServices;
        private readonly AuctionServices _auctionServices;

		public IndexModel(JoinedAuctionServices joinedAuctionServices, AccountServices accountServices, AuctionServices auctionServices)
		{
			_joinedAuctionServices = joinedAuctionServices;
			_accountServices = accountServices;
			_auctionServices = auctionServices;
		}
        [BindProperty]
		public List<JoinedAuction> JoinedAuction { get;set; } = default!;
        public Auction Auction { get; set; }
        public async Task<IActionResult> OnGetAsync(int? auctionId)
        {
            if(auctionId == null)
                return NotFound();
            try
            {
                await PopulateData(auctionId.Value);
                return Page();
            }catch (Exception ex) 
            {
                Console.WriteLine(ex.Message);
                return NotFound();
            }
            return Page();
        }
        private async Task PopulateData(int auctionId)
        {
            var tryGetAuctions = (await _auctionServices.GetWithCondition(a => a.AuctionId == auctionId, includeProperties: "JoinedAccounts.Account")).FirstOrDefault();
            if (tryGetAuctions == null)
                throw new NullReferenceException();
            Auction = tryGetAuctions;
            JoinedAuction = tryGetAuctions.JoinedAccounts.ToList();
        }
    }
}
