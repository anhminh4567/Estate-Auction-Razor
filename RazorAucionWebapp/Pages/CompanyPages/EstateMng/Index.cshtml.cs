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

namespace RazorAucionWebapp.Pages.CompanyPages.EstateMng
{
    public class IndexModel : PageModel
    {
        private readonly EstateServices _estateServices;

        public IndexModel(EstateServices context)
        {
            _estateServices = context;
        }

        public IList<Estate> Estates { get;set; } = default!;

        public async Task OnGetAsync()
        {
            Estates = await _estateServices.GetAll();
        }
    }
}
