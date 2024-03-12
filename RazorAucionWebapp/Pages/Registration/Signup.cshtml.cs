using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorAucionWebapp.Configure;
using Repository.Database.Model.AppAccount;
using Service.Services.AppAccount;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Security.Claims;

namespace RazorAucionWebapp.Pages.Registration
{
    public class SignupModel : PageModel
    {
		private readonly AccountServices _accountServices;
		private readonly BindAppsettings _bindAppsettings;
		private readonly AccountImagesServices _accountImagesServices;
		public SignupModel(AccountServices accountServices, BindAppsettings bindAppsettings, AccountImagesServices accountImagesServices)
		{
			_accountServices = accountServices;
			_bindAppsettings = bindAppsettings;
			_accountImagesServices = accountImagesServices;
		}

		[BindProperty]
		[Required]
		[DataType(DataType.EmailAddress)]
		public string Email { get; set; }
		[BindProperty]
		[Required]
		public string Name { get; set; }
		[BindProperty]
		[RegularExpression(@"^\d{10}$")]
		[Required]
		public string Tel { get; set; }
		
		[BindProperty]
		[RegularExpression(@"^\d{9}(?:\d{3})?$")]
		[Required]
		public string CMND { get; set; }
		[BindProperty]
		[Required]
		[DataType(DataType.Date)]
		public DateTime Dob { get; set; }
		[BindProperty]
		[Required]
		public string Password { get; set; }
		[BindProperty]
		[Required]
		public string RePassword { get; set; }
		public void OnGet()
        {
        }
		public async Task<IActionResult> OnPostAsync() 
		{
			if (await _accountServices.IsEmailExisted(Email))
			{
				ModelState.AddModelError(string.Empty,"email exist");
				return Page();
			}
			if(Email.Equals(_bindAppsettings.Admin.Email,StringComparison.CurrentCultureIgnoreCase)) {
				ModelState.AddModelError(string.Empty, "email exist");
				return Page();
			}
			var newAcc = new Account()
			{
				Email = Email,
				Password = Password,
				FullName = Name,
				Dob = Dob,
				Telephone = Tel,
				Status = Repository.Database.Model.Enum.AccountStatus.ACTIVED,
				Role = Repository.Database.Model.Enum.Role.CUSTOMER,
				CMND = CMND,
				Balance = 0,
			};
			var result = await _accountServices.Create(newAcc);
			if (result is null)
			{
				ModelState.AddModelError(string.Empty, "error on create new account");
				return Page();
			}
			await SetUserIdentity(newAcc);
			TempData["SuccessSignUp"] = $"Create success, email: {result.Email} , Id: {result.AccountId}";
			return RedirectToPage("/Index");
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
        private async Task<string> GetAvatar(int id)
        {
            var path = "~/PublicImages/";
            var Image = await _accountImagesServices.GetAccountAvatar(id);
            if (Image is null) path += "general/user_icon.png";
            else path += Path.Combine("storage", Image.Path);
            return path;
        }
    }
}
