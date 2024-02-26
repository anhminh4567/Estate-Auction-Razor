using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Repository.Database;
using Repository.Database.Model.AuctionRelated;

namespace RazorAucionWebapp.Pages.CustomerPages
{
    public class ViewAuctionListModel : PageModel
    {
        private readonly Repository.Database.AuctionRealEstateDbContext _context;

        public ViewAuctionListModel(Repository.Database.AuctionRealEstateDbContext context)
        {
            _context = context;
        }

        public IList<Auction> Auction { get;set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.Auctions != null)
            {
                Auction = await _context.Auctions
                .Include(a => a.Estate).ToListAsync();
            }
        }
    }
}
