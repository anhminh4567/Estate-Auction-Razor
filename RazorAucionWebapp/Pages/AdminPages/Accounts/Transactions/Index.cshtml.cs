using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Repository.Database;
using Repository.Database.Model;
using Service.Services;
using Service.Services.AppAccount;

namespace RazorAucionWebapp.Pages.AdminPages.Accounts.Transactions
{
    public class IndexModel : PageModel
    {
        private readonly TransactionServices _transactionServices;
        private readonly AccountServices _accountServices;

        public IndexModel(TransactionServices transactionServices, AccountServices accountServices)
        {
            _transactionServices = transactionServices;
            _accountServices = accountServices;
        }

        public List<Transaction>? Transactions { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? accountId)
        {
           if(accountId == null)
            {
                return BadRequest();
            }
            try
            {
                await PopulateData(accountId.Value);
                ViewData["accountId"] = accountId.Value;
                return Page();
            }catch(Exception ex) 
            {
                return BadRequest(ex.Message);
            }

        }
        private async Task PopulateData(int accountId)
        {
            Transactions = await _transactionServices.GetByAccountId(accountId);
            if(Transactions == null)
                Transactions = new List<Transaction>();
        }
    }
}
