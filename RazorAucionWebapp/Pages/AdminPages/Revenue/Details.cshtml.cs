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
    public class DetailsModel : PageModel
    {
        private readonly Repository.Database.AuctionRealEstateDbContext _context;

        public DetailsModel(Repository.Database.AuctionRealEstateDbContext context)
        {
            _context = context;
        }

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
    }
}
