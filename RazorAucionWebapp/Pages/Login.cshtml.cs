using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace RazorAucionWebapp.Pages
{
    public class LoginModel : PageModel
	{
		[BindProperty]
        [NotNull]
        [Required]
		public string? Username { get; set; }
		[BindProperty]
		[NotNull]
		[Required]
		public string? Password { get; set; }
		public void OnGet()
        {
        }
        public async Task<IActionResult> OnPostAsync() 
        {

            return Page();
        }
    }
}
