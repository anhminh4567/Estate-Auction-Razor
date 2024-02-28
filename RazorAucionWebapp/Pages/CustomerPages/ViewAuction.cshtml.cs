using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Repository.Database;
using Repository.Database.Model.AuctionRelated;
using Service.Services.Auction;

namespace RazorAucionWebapp.Pages.CustomerPages
{
   
    public class ViewAuctionListModel : PageModel
    {
        private readonly AuctionServices _auctionServices;

        public ViewAuctionListModel(AuctionServices auctionServices)
        {
            _auctionServices = auctionServices;
        }

        public IList<Auction> Auctions { get;set; } = default!;

        public async Task OnGetAsync()
        {
            await PopulateData();   
        }
		private int UserId { get; set; }

		private void GetUserId()
        {
            var result = int.TryParse(HttpContext.User.Claims.FirstOrDefault(c => c.Type.Equals("Id"))?.Value, out int userId);
            if(result is false) throw new Exception("Unauthorized User");
            UserId = userId;
        }
        private async Task PopulateData() 
        {
            Auctions = await _auctionServices.GetActiveAuctions();
		}
    }
}
