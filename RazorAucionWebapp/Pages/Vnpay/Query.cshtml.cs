using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Repository.Database.Model;
using Service.Services;
using Service.Services.VnpayService.VnpayUtility;

namespace RazorAucionWebapp.Pages.Vnpay
{
	public class QueryModel : PageModel
	{
		private VnpayQuery _vnpayQuery;
		private readonly TransactionServices _transactionServices;

		public QueryModel(VnpayQuery vnpayQuery, TransactionServices transactionServices)
		{
			_vnpayQuery = vnpayQuery;
			_transactionServices = transactionServices;
		}

		public async Task<IActionResult> OnGet(int? transactionId)
		{
			if(transactionId is null) {
				return BadRequest();
			}
			var getTransaction = await _transactionServices.GetById(transactionId.Value);
			_vnpayQuery.btnQuery_Click(HttpContext, getTransaction);
			return Page();
		}
	}
}
