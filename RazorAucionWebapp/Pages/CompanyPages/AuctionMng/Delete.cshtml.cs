using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Repository.Database;
using Repository.Database.Model.AuctionRelated;

namespace RazorAucionWebapp.Pages.CompanyPages.AuctionMng
{
    public class DeleteModel : PageModel
    {
        private readonly Repository.Database.AuctionRealEstateDbContext _context;

        public DeleteModel(Repository.Database.AuctionRealEstateDbContext context)
        {
            _context = context;
        }

        [BindProperty]
      public Auction Auction { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Auctions == null)
            {
                return NotFound();
            }

            var auction = await _context.Auctions.FirstOrDefaultAsync(m => m.AuctionId == id);

            if (auction == null)
            {
                return NotFound();
            }
            else 
            {
                Auction = auction;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null || _context.Auctions == null)
            {
                return NotFound();
            }
            var auction = await _context.Auctions.FindAsync(id);

            if (auction != null)
            {
                Auction = auction;
                _context.Auctions.Remove(Auction);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
