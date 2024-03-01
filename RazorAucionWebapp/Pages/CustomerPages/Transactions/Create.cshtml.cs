using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Repository.Database;
using Repository.Database.Model;
using Service.Services;

namespace RazorAucionWebapp.Pages.CustomerPages.Transactions
{
	public class CreateModel : PageModel
	{
		private readonly TransactionServices _transactionServices;

		public CreateModel(TransactionServices transactionServices)
		{
			_transactionServices = transactionServices;
		}

		public IActionResult OnGet()
		{
			return Page();
		}

		[BindProperty]
		public Transaction Transaction { get; set; } = default!;


		// To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
		public async Task<IActionResult> OnPostAsync()
		{
			if (!ModelState.IsValid)
			{
				return Page();
			}
			return RedirectToPage("./Index");
		}
	}
}
