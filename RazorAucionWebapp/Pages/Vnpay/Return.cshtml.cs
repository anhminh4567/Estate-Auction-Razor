using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Service.Services.VnpayService.Model;
using Service.Services.VnpayService.VnpayUtility;

namespace RazorAucionWebapp.Pages.Vnpay
{
    public class ReturnModel : PageModel
    {
        private VnpayReturn _vnpayReturnService;
        public VnpayReturnResult? VnpayResult { get; set; }
		public ReturnModel(VnpayReturn vnpayReturn)
		{
            this._vnpayReturnService = vnpayReturn;
		}

		public IActionResult OnGet()
        {
            var returnResult = _vnpayReturnService.OnTransactionReturn(HttpContext);
            if(returnResult is null) 
            {
                ModelState.AddModelError(string.Empty,"something wrong, why null, why empty, check code implementation");
            }
            VnpayResult = returnResult;
            return Page();
        }
    }
}
