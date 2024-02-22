using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RazorAucionWebapp.Pages
{
    [Authorize]
    public class UnauthorizedModel : PageModel
    {
        public void OnGet()
        {
            var userDisplayName = HttpContext.User.Identity.Name;
            ModelState.AddModelError(string.Empty,$"user {userDisplayName} is not authorized, go back to home page now");
        }
        public IActionResult OnPost() 
        {
            return LocalRedirect("/Index");
        }
    }
}
