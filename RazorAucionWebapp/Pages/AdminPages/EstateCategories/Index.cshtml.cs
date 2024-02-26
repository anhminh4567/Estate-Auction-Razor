using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Repository.Database;
using Repository.Database.Model.RealEstate;
using Repository.Implementation.RealEstate;
using Service.Services.RealEstate;

namespace RazorAucionWebapp.Pages.AdminPages.EstateCategories
{
    public class IndexModel : PageModel
    {
        private readonly EstateCategoryDetailServices _estateCategoryDetailRepository;

        public IndexModel(EstateCategoryDetailServices estateCategoryDetailRepository)
        {
            _estateCategoryDetailRepository = estateCategoryDetailRepository;
        }
        public IList<EstateCategoryDetail> EstateCategoryDetail { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync()
        {
            EstateCategoryDetail = await _estateCategoryDetailRepository.GetAll();
            return Page();
        }
    }
}
