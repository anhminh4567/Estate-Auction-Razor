using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Repository.Database;
using Repository.Database.Model.AuctionRelated;
using Service.Services.AppAccount;
using Service.Services.Auction;

namespace RazorAucionWebapp.Pages.CustomerPages
{
   
    public class ViewAuctionListModel : PageModel
    {
        private readonly AuctionServices _auctionServices;
        private readonly AccountServices _accountServices;

        public ViewAuctionListModel(AuctionServices auctionServices, AccountServices accountServices)
        {
            _auctionServices = auctionServices;
            _accountServices = accountServices;
        }

        public IList<Auction?> JoinedAuctions { get;set; } = default!;

        public async Task<IActionResult> OnGetAsync()
        {
            await PopulateData();
            return Page();
        }
        public async Task<IActionResult> OnPostAsync() 
        {
            await PopulateData();
            return Page();
        }
		private int UserId { get; set; }

		private void GetUserId()
        {
            var result = int.TryParse(HttpContext.User.Claims.FirstOrDefault(c => c.Type.Equals("Id"))?.Value, out int userId);
            if(result is false) 
                throw new Exception("Unauthorized User");
            UserId = userId;
        }
        private async Task PopulateData() 
        {
            GetUserId();
            var getUserDetail = await _accountServices.GetInclude(UserId, "JoinedAuctions.Auction.AuctionReceipt");
            if (getUserDetail is null)
                throw new Exception("user not exists");
            JoinedAuctions = getUserDetail.JoinedAuctions?.Select(j => j.Auction).ToList();
		}
    }
}
