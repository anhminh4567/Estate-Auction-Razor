using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Repository.Database.Model;
using Service.Services;
using System.Security.Claims;
using System.Text.Json;

namespace RazorAucionWebapp.Pages
{
    [ValidateAntiForgeryToken(Order = 1000)]
    [Authorize(Policy = "ADMIN_CUSTOMER_COMPANY")]
    public class MailBoxModel : PageModel
    {
        public List<Notification> Notifications { get; set; } = default!;
        private readonly NotificationServices _notificationServices;
        public MailBoxModel(NotificationServices notificationServices) 
        { 
            _notificationServices = notificationServices;
        }

        public async Task OnGetAsync()
        {
            await GetUserMail();
        }
        public async Task<IActionResult> OnGetCheckedAsync(int nId)
        {
            await _notificationServices.SetToChecked(nId);
            return new JsonResult("");
        }
        private async Task GetUserMail()
        {
            int id;
            var flag = int.TryParse(HttpContext.User.FindFirst("Id")?.Value, out id);
            if(flag) Notifications = await _notificationServices.GetAllNotification(id);
        }
        
    }
}
