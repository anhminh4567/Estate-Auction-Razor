using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Repository.Database.Model.AppAccount;
using Repository.Database.Model.Enum;
using Repository.Interfaces.AppAccount;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Security.Claims;

namespace RazorAucionWebapp.Pages.Registration
{
	public class LoginModel : PageModel
    {
        private readonly IAccountRepository _account;
        public LoginModel(IAccountRepository account)
        {
            _account = account;
        }

        [BindProperty]
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [BindProperty]
        [Required]
        public string Password { get; set; }
        public void OnGet()
        {
        }
        public async Task<IActionResult> OnPostAsync()
        {
            var flag = ModelState.IsValid;
            if (flag)
            {
                var tryGetAccount = await _account.GetByEmailPassword(Email, Password);
                if (tryGetAccount is null)
                {
                    ModelState.AddModelError(string.Empty, "user not found in database, try again");
                    return Page();
                }
                //var tryGetAccount = new Account() { Email = Email, Password = Password, Role = Role.CUSTOMER };
                await SetUserIdentity(tryGetAccount);
                return LocalRedirect("/Index");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Incorrect form");
            }
            return Page();
        }
        private async Task SetUserIdentity(Account account)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email,account.Email),
                new Claim(ClaimTypes.Role, account.Role.ToString()),
            };
            var claimIdentity = new ClaimsIdentity(claims, "cookie", ClaimTypes.Email, ClaimTypes.Role);
            var claimPrinciple = new ClaimsPrincipal(claimIdentity);
            await HttpContext.SignInAsync("cookie", claimPrinciple);
        }
    }
}
