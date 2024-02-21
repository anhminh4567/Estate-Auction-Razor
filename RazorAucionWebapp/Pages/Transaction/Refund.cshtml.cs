using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Service.Services.VnpayService;
using Repository.Database.Model.AppAccount;
using Service.Services.VnpayService.Model;

namespace RazorAucionWebapp.Pages.Transaction
{
    public class RefundModel : PageModel
    {
        private VnpayRefund _vnpayRefund;
        private VnpayQuery _vnpayQuery;
		public RefundModel()
		{
            this._vnpayRefund = new VnpayRefund();
            this._vnpayQuery = new VnpayQuery();
		}

		public IActionResult OnGet()
        {
            var queryResult = _vnpayQuery.btnQuery_Click(HttpContext, new Repository.Database.Model.Transaction() { vnp_TxnRef = "638441328050377825", vnp_TransactionDate = 20240221172044 });
            //query ko can, do no chi can 3 tham so chinh la amount, txnref, paydate, deu dc luu trong db, just testing
            var refundResult = _vnpayRefund.btnRefund_Click(context: HttpContext, queryResult: new VnpayQueryResult() { vnp_Amount = "100000", vnp_TxnRef = "638441328050377825", vnp_PayDate = "20240221172044" },account: new Account() { FullName = "Test Ting"});
            return Page();
        }
    }
}
