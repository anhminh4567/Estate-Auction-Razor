using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorAucionWebapp.Configure;
using Repository.Database.Model.AppAccount;
using Repository.Database.Model.Enum;
using Repository.Interfaces.AppAccount;
using Service.MyHub.HubServices;
using Service.Services;
using Service.Services.AppAccount;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Claims;

namespace RazorAucionWebapp.Pages.Registration
{
	public class LoginModel : PageModel
    {
        private readonly AccountServices _accountServices;
        private readonly AccountImagesServices _accountImagesServices;
        private readonly NotificationServices _notificationServices;
        private readonly NotificationHubService _notificationHubService;
        private readonly BindAppsettings _bindAppsettings;

		public LoginModel(
            AccountServices accountServices, 
            AccountImagesServices accountImagesServices, 
            NotificationServices notificationServices, 
            NotificationHubService notificationHubService,
            BindAppsettings bindAppsettings)
		{
			_accountServices = accountServices;
			_accountImagesServices = accountImagesServices;
            _notificationServices = notificationServices;
            _notificationHubService = notificationHubService;
			_bindAppsettings = bindAppsettings;
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
                //admin in appsettings
                if(Email == _bindAppsettings.Admin.Email && Password == _bindAppsettings.Admin.Password)
                {
                    var adminAccount = new Account() 
                    {
                        Email = _bindAppsettings.Admin.Email,
                        Role = Role.ADMIN,
                        Status =  AccountStatus.ACTIVED,
                        FullName = "ADMIN USER",
                        AccountId = 0
                    };
                    await SetUserIdentity(adminAccount);
                    return RedirectToPage("/AdminPages/Accounts/Index");
				}
                var getAccount = await _accountServices.GetByEmailPassword(Email, Password);
                if (getAccount is null)
                {
                    ModelState.AddModelError(string.Empty, "user not found in database, try again");
                    return Page();
                }
                if(getAccount.Status.Equals(AccountStatus.DEACTIVED)) 
                {
                    ModelState.AddModelError(string.Empty, "user account is DEACTIVED, contact admin to unlock");
                    return Page();
                }
                await SetUserIdentity(getAccount);
                await CheckMail(getAccount.AccountId, getAccount.Email);
                TempData["SuccessLogin"] = "Login Success for user " + getAccount.Email;
                //if (getAccount.Role.Equals(Role.ADMIN))
                //{
                //    return RedirectToPage("/AdminPages/Accounts/Index");
                //}
                return RedirectToPage("/Index");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Incorrect form");
            }
            return Page();
        }
        private async Task<string> GetAvatar(int id)
        {
            var path = "~/PublicImages/";
            var Image = await _accountImagesServices.GetAccountAvatar(id);
            if (Image is null) path += "general/user_icon.png";
            else path += Path.Combine("storage", Image.Path);
            return path;
        }
        private async Task SetUserIdentity(Account account)
        {

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email,account.Email),
                new Claim(ClaimTypes.Role, account.Role.ToString()),
                new Claim(ClaimTypes.Name,account.FullName),
                new Claim("Status",account.Status.ToString()),
                new Claim("Id",account.AccountId.ToString()),
                new Claim("Avatar", await GetAvatar(account.AccountId))
            };
            var claimIdentity = new ClaimsIdentity(claims, "cookie", ClaimTypes.Email, ClaimTypes.Role);
            var claimPrinciple = new ClaimsPrincipal(claimIdentity);
            await HttpContext.SignInAsync("cookie", claimPrinciple);
        }
        private async Task CheckMail(int id, string email)
        {
            if (await _notificationServices.CheckUnreadMail(id))
            {
                TempData["NewMail"] = true;
            }
        }
    }
}
