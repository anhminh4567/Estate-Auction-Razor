using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Repository.Database;
using Repository.Database.Model.AppAccount;
using Repository.Database.Model.Enum;
using Service.Services.AppAccount;

namespace RazorAucionWebapp.Pages.AdminPages.Accounts
{
    public class IndexModel : PageModel
    {
        private readonly AccountServices _accountServices;

        public IndexModel(AccountServices accountServices)
        {
            _accountServices = accountServices;
        }

        public IList<Account> Accounts { get;set; } = default!;

        public async Task<IActionResult> OnGetAsync()
        {
            await PopulateData();
            return Page();
        }
        private async Task PopulateData()
        {
            Accounts = await _accountServices.GetAllCustomers();
        }
    }
}
