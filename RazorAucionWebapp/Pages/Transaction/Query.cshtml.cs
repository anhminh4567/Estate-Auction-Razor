using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Repository.Database.Model;
using Service.Services.VnpayService;

namespace RazorAucionWebapp.Pages.Transaction
{
    public class QueryModel : PageModel
    {
        private VnpayQuery _vnpayQuery;
		public QueryModel()
		{
            _vnpayQuery = new VnpayQuery();
		}
        
		public IActionResult OnGet()
        {
            _vnpayQuery.btnQuery_Click(HttpContext,new Repository.Database.Model.Transaction() { vnp_TxnRef = "638441328050377825", vnp_TransactionDate = 20240221172044 });
			return Page();
        }
    }
}
