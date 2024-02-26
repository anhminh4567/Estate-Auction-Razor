using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Repository.Database;
using Repository.Database.Model.RealEstate;

namespace RazorAucionWebapp.Pages.CompanyPages.EstateMng
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
        ViewData["CompanyId"] = new SelectList(_context.Companys, "AccountId", "CMND");
            return Page();
        }

        [BindProperty]
        public Estate Estate { get; set; } = default!;

        // added property
        [BindProperty]
        [Required]
        public string EstateCategories { get; set; }

        [BindProperty]
        [Required]
        public string ImageUrl { get; set; }


        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
          if (!ModelState.IsValid || _context.Estates == null || Estate == null)
            {
                return Page();
            }

            _context.Estates.Add(Estate);
            await _context.SaveChangesAsync();

            //return RedirectToPage("./Index");
            return Page();
        }
    }
}
