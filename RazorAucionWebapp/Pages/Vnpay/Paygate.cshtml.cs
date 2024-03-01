using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Repository.Database.Model.AppAccount;
using Service.Services.VnpayService.VnpayUtility;

namespace RazorAucionWebapp.Pages.Vnpay
{
	public class PaygateModel : PageModel
	{
		private VnpayBuildUrl _vnpayUrlBuilder;
		private IAccountRepository _accountRepository;
		public PaygateModel()
		{
			_vnpayUrlBuilder = new VnpayBuildUrl();
		}
		//test only
		public IActionResult OnGet()
		{
			var dummyAccount = new Account()
			{
				Email = "testingwebandstuff@gmail.com",
				Telephone = "12345667890",
				FullName = "testing web andstuff",
			};
			var urlString = _vnpayUrlBuilder.btnPay_Click(HttpContext, dummyAccount, 100000, "");
			if (urlString == null || string.IsNullOrEmpty(urlString))
			{
				ModelState.AddModelError(string.Empty, "something wrong check again");
				return Page();
			}
			return Redirect(urlString);
		}
		public async Task<IActionResult> OnPost(int accountId, int amount)
		{
			var getAccount = await _accountRepository.GetAsync(accountId);
			var urlString = _vnpayUrlBuilder.btnPay_Click(HttpContext, getAccount, amount);
			if (urlString == null || string.IsNullOrEmpty(urlString))
			{
				ModelState.AddModelError(string.Empty, "something wrong check again");
				return Page();
			}
			return Redirect(urlString);
		}
	}
}
