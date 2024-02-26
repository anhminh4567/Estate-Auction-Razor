using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Repository.Database;
using Repository.Database.Model.AuctionRelated;

namespace RazorAucionWebapp.Pages.CustomerPages
{
    public class BidAuctionModel : PageModel
    {
        private readonly Repository.Database.AuctionRealEstateDbContext _context;

        public BidAuctionModel(Repository.Database.AuctionRealEstateDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Bid Bid { get; set; } = default!;
        [BindProperty]
        public Auction Auction { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? auctionId)
        {
            if (auctionId == null)
            {
                return NotFound();
            }

            var auction = await _context.Auctions.FirstOrDefaultAsync(a => a.AuctionId == auctionId);
            if (auction == null)
            {
                return NotFound();
            }
            else
            {
                Auction = auction;
            }
            //ViewData["AuctionId"] = new SelectList(_context.Auctions, "AuctionId", "AuctionId");
            //ViewData["BidderId"] = new SelectList(_context.Accounts, "AccountId", "CMND");
            return Page();
        }

        

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid || _context.BidLogs == null || Bid == null)
            {
                return Page();
            }

            //_context.BidLogs.Add(Bid);
            //await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
