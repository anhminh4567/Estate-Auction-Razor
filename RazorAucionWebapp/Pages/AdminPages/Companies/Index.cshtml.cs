using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Repository.Database;
using Repository.Database.Model.AppAccount;
using Service.Services.AppAccount;

namespace RazorAucionWebapp.Pages.AdminPages.Companies
{
    public class IndexModel : PageModel
    {
        private readonly CompanyServices _companyServices;
        private readonly AccountServices _accountServices;

        public IndexModel(CompanyServices companyServices, AccountServices accountServices)
        {
            _companyServices = companyServices;
            _accountServices = accountServices;
        }
        public IList<Company> Companies { get;set; } = default!;
        public async Task OnGetAsync()
        {
            Companies = await _companyServices.GetAll();
        }
    }
}
