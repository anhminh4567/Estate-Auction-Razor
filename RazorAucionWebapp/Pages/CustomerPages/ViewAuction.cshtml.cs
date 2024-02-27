using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Repository.Database;
using Repository.Database.Model.AuctionRelated;
using Service.Services.AuctionService;

namespace RazorAucionWebapp.Pages.CustomerPages
{
    public class ViewAuctionListModel : PageModel
    {
        private readonly AuctionServices _auctionServices;

        public ViewAuctionListModel(AuctionServices auctionServices)
        {
            _auctionServices = auctionServices;
        }

        public IList<Auction> Auction { get;set; } = default!;

        public async Task OnGetAsync()
        {
        }
    }
}
