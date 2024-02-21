using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Repository.Database.Model.AppAccount;
using Repository.Database.Model.Enum;
using Service.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Security.Claims;

namespace RazorAucionWebapp.Pages
{
	public class LoginModel : PageModel
	{
		private readonly IAccountRepository _accountRepository;
		public LoginModel(IAccountRepository accountRepository)
		{
			this._accountRepository = accountRepository;
		}

		[BindProperty]
		[NotNull]
		[Required]
		public string? Email { get; set; }
		[BindProperty]
		[NotNull]
		[Required]
		public string? Password { get; set; }
		public void OnGet()
		{
		}
		public async Task<IActionResult> OnPostAsync()
		{
			var tryGetAccount = await _accountRepository.GetByEmailPassword(Email, Password);
			if (tryGetAccount is null)
			{
				ModelState.AddModelError("user not found", "user not found in database, try again");
				return Page();
			}
			//var tryGetAccount = new Account() { Email = Email, Password = Password, Role = Role.CUSTOMER };
			await SetUserIdentity(tryGetAccount);
			return LocalRedirect("/Index");
		}
		private async Task SetUserIdentity(Account account)
		{
			var claims = new List<Claim>
			{
				new Claim(ClaimTypes.Email,account.Email),
				new Claim(ClaimTypes.Role, account.Role.ToString()),
			};
			var claimIdentity = new ClaimsIdentity(claims,"cookie",ClaimTypes.Email,ClaimTypes.Role);
			var claimPrinciple = new ClaimsPrincipal(claimIdentity);
			await HttpContext.SignInAsync("cookie",claimPrinciple);
		}
	}
}
