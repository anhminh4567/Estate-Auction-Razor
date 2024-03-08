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

namespace RazorAucionWebapp.Pages.AdminPages.Accounts
{
    public class DetailModel : PageModel
    {
        private readonly AccountServices _accountServices;

        public DetailModel(AccountServices accountServices)
        {
            _accountServices = accountServices;
        }

        public Account Account { get; set; } = default!; 

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null )
            {
                return NotFound();
            }
            try
            {
                await PopulateData(id.Value);
            }catch (Exception ex) 
            {
                return BadRequest(ex.Message);
            }

            return Page();
        }
        private async Task PopulateData(int accountId)
        {
            Account = await _accountServices.GetById(accountId) ?? throw new Exception("cannot find id for this account");
        }
    }
}
