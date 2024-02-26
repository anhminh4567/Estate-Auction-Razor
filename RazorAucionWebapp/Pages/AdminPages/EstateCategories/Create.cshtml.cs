using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Repository.Database;
using Repository.Database.Model.RealEstate;
using Repository.Implementation.RealEstate;
using Service.Services.RealEstate;

namespace RazorAucionWebapp.Pages.AdminPages.EstateCategories
{
    public class CreateModel : PageModel
    {
        private readonly EstateCategoryDetailServices _estateCategoryDetailRepository;

        public CreateModel(EstateCategoryDetailServices estateCategoryDetailRepository)
        {
            _estateCategoryDetailRepository = estateCategoryDetailRepository;
        }
        public IActionResult OnGet()
        {
            return Page();
        }
        [BindProperty]
        public EstateCategoryDetail EstateCategoryDetail { get; set; } = default!;
        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid is false || EstateCategoryDetail is null)
            {
                return Page();
            }
            await _estateCategoryDetailRepository.Create(EstateCategoryDetail);
            return RedirectToPage("./Index");
        }
    }
}
