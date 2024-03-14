using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Repository.Database;
using Repository.Database.Model.AuctionRelated;

namespace RazorAucionWebapp.Pages.AdminPages.Revenue
{
    public class DeleteModel : PageModel
    {
        private readonly Repository.Database.AuctionRealEstateDbContext _context;

        public DeleteModel(Repository.Database.AuctionRealEstateDbContext context)
        {
            _context = context;
        }

        [BindProperty]
      public AuctionReceipt AuctionReceipt { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.AuctionReceipts == null)
            {
                return NotFound();
            }

            var auctionreceipt = await _context.AuctionReceipts.FirstOrDefaultAsync(m => m.ReceiptId == id);

            if (auctionreceipt == null)
            {
                return NotFound();
            }
            else 
            {
                AuctionReceipt = auctionreceipt;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null || _context.AuctionReceipts == null)
            {
                return NotFound();
            }
            var auctionreceipt = await _context.AuctionReceipts.FindAsync(id);

            if (auctionreceipt != null)
            {
                AuctionReceipt = auctionreceipt;
                _context.AuctionReceipts.Remove(AuctionReceipt);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
