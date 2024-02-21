using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Repository.Database.Model.AppAccount;
using Service.Services.VnpayService;

namespace RazorAucionWebapp.Pages.Transaction
{
    public class PaygateModel : PageModel
    {
        private BuildUrl _vnpayUrlBuilder;
		public PaygateModel()
		{
            _vnpayUrlBuilder = new BuildUrl(100000,"");
		}

		public IActionResult OnGet()
        {
            var dummyAccount = new Account()
            {
                Email = "testingwebandstuff@gmail.com",
                Telephone = "12345667890",
                FullName = "testing web andstuff",
            };
            var urlString = _vnpayUrlBuilder.btnPay_Click(HttpContext, dummyAccount);
            if (urlString == null || string.IsNullOrEmpty(urlString)) 
            {
                ModelState.AddModelError(string.Empty,"something wrong check again");
                return Page();
            }
            return Redirect(urlString);
        }
    }
}
