using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Repository.Database;
using Repository.Database.Model;
using Repository.Database.Model.AppAccount;
using Service.Services;

namespace RazorAucionWebapp.Pages.AdminPages.Accounts.Transactions
{
    public class RefundModel : PageModel
    {
        private readonly TransactionServices _transactionServices;
        private readonly VnpayAvailableServices _vnpayAvailableServices;
        public RefundModel(TransactionServices transactionServices, VnpayAvailableServices vnpayAvailableServices)
        {
            _transactionServices = transactionServices;
            _vnpayAvailableServices = vnpayAvailableServices;
        }

        [BindProperty]
        public Transaction Transaction { get; set; } = default!;
        private Account User { get; set; }
        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
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

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            try
            {
                await PopulateData(id.Value);
                var result =await  _vnpayAvailableServices.AdminRefundVnpay(HttpContext, Transaction, User);
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
        public async Task PopulateData(int transactionId)
        {
            Transaction = await _transactionServices.GetInclude(transactionId, "Account") ?? throw new Exception("cannot found transactin with this id");
            User = Transaction.Account ?? throw new Exception("cannot found owner of this transaction, something is really wrong");
        }
    }
}
