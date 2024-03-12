using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RazorAucionWebapp.Pages
{
    public class UnauthorizedModel : PageModel
    {
        public void OnGet(string? message)
        {
            var userDisplayName = HttpContext.User.Identity.Name;
            ModelState.AddModelError(string.Empty,$"user {userDisplayName} is not authorized, go back to home page now");
            if(message != null)
            {
                ModelState.AddModelError(string.Empty,message + " ||  from user: ");
            }
        }
        public IActionResult OnPost() 
        {
            return LocalRedirect("/Index");
        }
    }
}
