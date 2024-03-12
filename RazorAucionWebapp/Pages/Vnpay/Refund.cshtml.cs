using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorAucionWebapp.Configure;
using Repository.Database.Model;
using Repository.Database.Model.AppAccount;
using Service.Services;
using Service.Services.AppAccount;
using Service.Services.VnpayService.Model;
using Service.Services.VnpayService.VnpayUtility;
using System.Diagnostics;

namespace RazorAucionWebapp.Pages.Vnpay
{
    public class RefundModel : PageModel
    {
        private readonly VnpayAvailableServices _vnpayAvailableService;
        private readonly TransactionServices _transactionServices;
        private readonly AccountServices _accountServices;
        private readonly BindAppsettings _bindAppsettings;

		public RefundModel(VnpayAvailableServices vnpayAvailableService, TransactionServices transactionServices, AccountServices accountServices, BindAppsettings bindAppsettings)
		{
			_vnpayAvailableService = vnpayAvailableService;
			_transactionServices = transactionServices;
			_accountServices = accountServices;
			_bindAppsettings = bindAppsettings;
		}

        private Transaction _transaction { get; set; }
        private Account _account { get; set; }
		public async Task<IActionResult> OnGet(int? transactionId, int? accountId)
        {
            if (transactionId is null || accountId is null || accountId == 0)
                return BadRequest();
            try
            {
                await PopulateData(transactionId.Value,accountId.Value);
                //var queryResult = _vnpayQuery.btnQuery_Click(HttpContext, new Repository.Database.Model.Transaction() { vnp_TxnRef = "638441328050377825", vnp_TransactionDate = 20240221172044 });
                //query ko can, do no chi can 3 tham so chinh la amount, txnref, paydate, deu dc luu trong db, just testing
                //var refundResult = _vnpayRefund.btnRefund_Click(context: HttpContext, 
                //    transaction: new Repository.Database.Model.Transaction() { vnp_Amount = "100000", vnp_TxnRef = "638441328050377825", vnp_PayDate = "20240221172044" }, account: new Account() { FullName = "Test Ting" });
                var refundResult =await  _vnpayAvailableService.RefundVnpayTransaction(
                    httpContext: HttpContext,
                    transaction: _transaction,
                    account: _account,
                    RefundValidTimeMinute: _bindAppsettings.RefundValidTime
                    );
                if (refundResult.isSuccess == false)
                {
                    ModelState.AddModelError(string.Empty, refundResult.message);
                    return Page();
					//throw new Exception(refundResult.message);
				}
				TempData["RefundSuccess"] = "Successfully refunded";
                return Page();
			}
			catch (Exception ex) 
            {
                return BadRequest(ex.Message);
            }
            
        }
        private async Task PopulateData(int transactionId, int accountId)
        {
            //var tryGetUserId = int.TryParse(HttpContext.User.Claims?.FirstOrDefault(c => c.Type.Equals("Id"))?.Value,out var userId);
            //if(tryGetUserId is false)
            //    throw new UnauthorizedAccessException("not authorized");
            _account = await _accountServices.GetById(accountId);

			var tryGetTransaction = await _transactionServices.GetById(transactionId);
            if (tryGetTransaction is null)
                throw new Exception("cannot find transaction");
            _transaction = tryGetTransaction;   
           
        }
    }
}
