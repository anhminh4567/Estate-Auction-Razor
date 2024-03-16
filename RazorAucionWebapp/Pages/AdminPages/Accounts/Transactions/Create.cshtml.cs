using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Repository.Database;
using Repository.Database.Model;
using Repository.Database.Model.AppAccount;
using Repository.Database.Model.Enum;
using Service.Interfaces;
using Service.Interfaces.AppAccount;
using Service.Services.AppAccount;

namespace RazorAucionWebapp.Pages.AdminPages.Accounts.Transactions
{
    public class CreateModel : PageModel
    {
        private readonly ITransactionServices _transactionServices;
        private readonly IAccountServices _accountServices;

		public CreateModel(ITransactionServices transactionServices, IAccountServices accountServices)
		{
			_transactionServices = transactionServices;
			_accountServices = accountServices;
		}
		//[BindProperty]
		//public Transaction Transaction { get; set; } = default!;
		[BindProperty]
		[Required]
        public decimal Amount { get; set; }
		[BindProperty]
		public string DetailDescription { get; set; } = "Transaction By Admin, admin create this to manually update balance, might be due to some transaction error before";
		[BindProperty]
		public int? AccId { get; set; }
		public IActionResult OnGet(int? accountId)
        {
			AccId = accountId;
			if (accountId == null)
			{
				return BadRequest();
			}
			try
			{
				return Page();
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

        
        

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync(int? accountId)
        {
			if (accountId == null)
			{
				return BadRequest();
			}
			try
			{
				var transaction = new Transaction()
				{
					AccountId = accountId,
					Status = TransactionStatus.SUCCESS,
					vnp_Amount = Amount.ToString(),
					vnp_OrderInfo = DetailDescription,
					vnp_PayDate = DateTime.Now.ToString(),
					vnp_TransactionDate = DateTime.Now.Ticks,
					vnp_TxnRef = "NONE, THIS IS LOCAL SYTEM GENERATED, CANNOT QUERY OR REFUND VNPAY",
				};
				var result = await _transactionServices.AdminCreateTransaction(transaction);
				if (result.IsSuccess)
				{
					return RedirectToPage("./Index", new { accountId = accountId});
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
		private Task PopulateData(int accountId)
		{
			return Task.CompletedTask;
		}


	}
}
