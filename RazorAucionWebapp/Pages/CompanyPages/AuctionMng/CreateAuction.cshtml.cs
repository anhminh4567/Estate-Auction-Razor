using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Repository.Database;
using Repository.Database.Model.AuctionRelated;
using Service.Services.AuctionService;
using Service.Services.RealEstate;

namespace RazorAucionWebapp.Pages.CompanyPages.AuctionMng
{
    public class CreateModel : PageModel
    {
        private readonly AuctionServices _auctionServices;
        private readonly EstateServices _estateServices;
        private readonly EstateCategoryDetailServices _estateCategoryDetailServices;
        public CreateModel(AuctionServices auctionServices, EstateServices estateServices, EstateCategoryDetailServices estateCategoryDetailServices) 
        {
            _auctionServices = auctionServices;
            _estateServices = estateServices;
            _estateCategoryDetailServices = estateCategoryDetailServices;
        }
        public async Task<IActionResult> OnGet()
        {
            ViewData["EstateId"] = new SelectList(await _estateCategoryDetailServices.GetAll(), "EstateId", "Description");
            return Page();
        }
        [BindProperty]
        public Auction Auction { get; set; } = default!;
        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {

          //if (!ModelState.IsValid || _context.Auctions == null || Auction == null)
          //  {
          //      return Page();
          //  }

            //_context.Auctions.Add(Auction);
            //await _context.SaveChangesAsync();

            //return RedirectToPage("./Index");
            return Page();
        }
    }
}
