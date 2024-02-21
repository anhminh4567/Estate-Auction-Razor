using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Service.Services.VnpayService;
using Service.Services.VnpayService.Model;

namespace RazorAucionWebapp.Pages.Transaction
{
    public class ReturnModel : PageModel
    {
        private VnpayReturn _vnpayReturnService;
        public VnpayReturnResult? VnpayResult { get; set; }
		public ReturnModel()
		{
            this._vnpayReturnService = new VnpayReturn();
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
