using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Repository.Database;
using Repository.Database.Model.RealEstate;
using Service.Services.RealEstate;

namespace RazorAucionWebapp.Pages.AdminPages.EstatesManage
{
    public class IndexModel : PageModel
    {
        private readonly EstateServices _estateServices;

        public IndexModel(EstateServices estateServices)
        {
            _estateServices = estateServices;
        }

        public List<Estate> Estate { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync()
        {
            await PopulateData();
            return Page();
        }
        private async Task PopulateData()
        {
            Estate = await _estateServices.GetAll();
        }
    }
}
