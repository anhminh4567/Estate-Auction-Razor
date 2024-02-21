using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace RazorAucionWebapp.Pages.Registration
{
    public class SignupModel : PageModel
    {
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

    }
}
