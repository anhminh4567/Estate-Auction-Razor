using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Repository.Database;
using Repository.Database.Model.AuctionRelated;

namespace RazorAucionWebapp.Pages.AdminPages.Revenue
{
    public class CreateModel : PageModel
    {
        private readonly Repository.Database.AuctionRealEstateDbContext _context;

        public CreateModel(Repository.Database.AuctionRealEstateDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
        ViewData["AuctionId"] = new SelectList(_context.Auctions, "AuctionId", "AuctionId");
        ViewData["BuyerId"] = new SelectList(_context.Accounts, "AccountId", "CMND");
            return Page();
        }

        [BindProperty]
        public AuctionReceipt AuctionReceipt { get; set; } = default!;
        

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
          if (!ModelState.IsValid || _context.AuctionReceipts == null || AuctionReceipt == null)
            {
                return Page();
            }

            _context.AuctionReceipts.Add(AuctionReceipt);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
