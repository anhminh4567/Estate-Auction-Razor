using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RazorAucionWebapp.Configure;
using Repository.Database;
using Repository.Database.Model;
using Service.Services;

namespace RazorAucionWebapp.Pages.CustomerPages.Transactions
{
    public class IndexModel : PageModel
    {
        private readonly TransactionServices _transactionServices;
		private readonly BindAppsettings _bindAppsettings;

		public IndexModel(TransactionServices transactionServices, BindAppsettings bindAppsettings)
		{
			_transactionServices = transactionServices;
			_bindAppsettings = bindAppsettings;
		}

		public List<Transaction> Transactions { get;set; } = default!;
		public int RefundValidTime { get; private set; }
		public int _userId { get; set; }
        public async Task<IActionResult> OnGetAsync()
        {
			try
			{
				await Poplulatedata();

				return Page();
			}
			catch (Exception ex) 
			{
				return BadRequest(ex.Message);
			}
           

        }
        private async Task Poplulatedata()
        {
			var tryGetUserId = int.TryParse(HttpContext.User.Claims.FirstOrDefault(c => c.Type.Equals("Id"))?.Value, out var userId);
			if (tryGetUserId is false)
				throw new UnauthorizedAccessException("User Unauthorized");
			_userId = userId;
			RefundValidTime = _bindAppsettings.RefundValidTime;
			Transactions = await _transactionServices.GetByAccountId(_userId);
		}
    }
}
