using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
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
        [BindProperty]
        public int AccountId { get; set; }
        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null )
            {
                return NotFound();
            }
            try
            {
                await PopulateData(id.Value);
                return Page();
            }
            catch (Exception ex) 
            {
                return BadRequest(ex.Message);
            }

            
        }
        public async Task<IActionResult> OnPostBanAsync()
        {
            if(AccountId == 0) 
            {
                return NotFound();
            }
            try
            {
                await PopulateData(AccountId);
                var result = await _accountServices.AdminBanUser(Account);
                if(result.IsSuccess ) 
                {
                    return RedirectToPage("./Index");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, result.message);
                    return Page();
                }
            }
            catch (Exception ex) 
            {
                return BadRequest(ex.Message);
            }
        }
        public async Task<IActionResult> OnPostActiveAsync()
        {
            if (AccountId == 0)
            {
                return NotFound();
            }
            try
            {
                await PopulateData(AccountId);
                var result = await _accountServices.AdminActiveUser(Account);
                if (result.IsSuccess)
                {
                    return RedirectToPage("./Index");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, result.message);
                    return Page();
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        private async Task PopulateData(int accountId)
        {
            Account = await _accountServices.GetById(accountId) ?? throw new Exception("cannot find id for this account");
        }
    }
}
