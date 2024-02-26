using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Repository.Database;
using Repository.Database.Model.AuctionRelated;

namespace RazorAucionWebapp.Pages.CompanyPages.AuctionMng
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
        ViewData["EstateId"] = new SelectList(_context.Estates, "EstateId", "Description");
            return Page();
        }

        [BindProperty]
        public Auction Auction { get; set; } = default!;
        

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
          if (!ModelState.IsValid || _context.Auctions == null || Auction == null)
            {
                return Page();
            }

            //_context.Auctions.Add(Auction);
            //await _context.SaveChangesAsync();

            //return RedirectToPage("./Index");
            return Page();
        }
    }
}
