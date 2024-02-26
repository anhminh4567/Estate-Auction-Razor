using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Repository.Database.Model.AppAccount;
using Service.Services.AppAccount;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace RazorAucionWebapp.Pages.Registration
{
    public class SignupModel : PageModel
    {
		private readonly AccountServices _accountServices;
		public SignupModel(AccountServices accountServices)
		{
			_accountServices = accountServices;
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
			var newAcc = new Account()
			{
				Email = Email,
				Password = Password,
				FullName = Name,
				Dob = Dob,
				Telephone = Tel,
				Status = Repository.Database.Model.Enum.AccountStatus.PENDING,
				IsVerified = 0,
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
			TempData["SuccessSignUp"] = $"Create success, email: {result.Email} , Id: {result.AccountId}";
			return RedirectToPage("/Index");
		}
    }
}
