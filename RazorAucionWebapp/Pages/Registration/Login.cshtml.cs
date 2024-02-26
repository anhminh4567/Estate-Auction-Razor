using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Repository.Database.Model.AppAccount;
using Repository.Database.Model.Enum;
using Repository.Interfaces.AppAccount;
using Service.Services.AppAccount;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Security.Claims;

namespace RazorAucionWebapp.Pages.Registration
{
	public class LoginModel : PageModel
    {
        private readonly AccountServices _accountServices;
        public LoginModel(AccountServices account)
        {
            _accountServices = account;
        }

        [BindProperty]
        [Required]
        [DataType(DataType.EmailAddress)]
        public string? Email { get; set; }
        [BindProperty]
        [Required]
        public string? Password { get; set; }
        public void OnGet()
        {
        }
        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var getAccount = await _accountServices.GetByEmailPassword(Email, Password);
                if (getAccount is null)
                {
                    ModelState.AddModelError(string.Empty, "user not found in database, try again");
                    return Page();
                }
                await SetUserIdentity(getAccount);
                TempData["SuccessLogin"] = "Login Success for user " + getAccount.Email;
                return RedirectToPage("/Index");
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
                new Claim(ClaimTypes.Name,account.FullName),
                new Claim("IsVerified", account.IsVerified == 0 ? "false" : "true"),
                new Claim("Status",account.Status.ToString()),
                new Claim("Id",account.AccountId.ToString()),
            };
            var claimIdentity = new ClaimsIdentity(claims, "cookie", ClaimTypes.Email, ClaimTypes.Role);
            var claimPrinciple = new ClaimsPrincipal(claimIdentity);
            await HttpContext.SignInAsync("cookie", claimPrinciple);
        }
    }
}
