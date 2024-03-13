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

namespace RazorAucionWebapp.Pages.AdminPages.Revenue
{
    public class EditModel : PageModel
    {
        private readonly Repository.Database.AuctionRealEstateDbContext _context;

        public EditModel(Repository.Database.AuctionRealEstateDbContext context)
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

            var auctionreceipt =  await _context.AuctionReceipts.FirstOrDefaultAsync(m => m.ReceiptId == id);
            if (auctionreceipt == null)
            {
                return NotFound();
            }
            AuctionReceipt = auctionreceipt;
           ViewData["AuctionId"] = new SelectList(_context.Auctions, "AuctionId", "AuctionId");
           ViewData["BuyerId"] = new SelectList(_context.Accounts, "AccountId", "CMND");
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(AuctionReceipt).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AuctionReceiptExists(AuctionReceipt.ReceiptId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool AuctionReceiptExists(int id)
        {
          return (_context.AuctionReceipts?.Any(e => e.ReceiptId == id)).GetValueOrDefault();
        }
    }
}
