using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Repository.Database.Model;
using Repository.Database.Model.AppAccount;
using Repository.Database.Model.AuctionRelated;
using Service.MyHub.HubServices;
using Service.Services.AppAccount;
using Service.Services.Auction;

namespace RazorAucionWebapp.Pages.CustomerPages
{
    public class QuitAuctionModel : PageModel
    {
        private readonly NotificationHubService _notificationHubService;
        private readonly JoinedAuctionServices _joinedAuctionServices;

        public QuitAuctionModel(NotificationHubService notificationHubService)
        {
            _notificationHubService = notificationHubService;
        }
        private int _userId { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            try
            {
                var result = await _joinedAuctionServices.QuitAuction(_userId, id.Value);
                if (result.IsSuccess)
                {
                    return RedirectToPage("../Index");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, result.message);
                    return Page();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest();
            }
        }

        public async Task SendMessage(string email, Notification notification)
        {
            await _notificationHubService.SendNewNotification(email, notification);
        }
        private void GetUserId()
        {
            var result = int.TryParse(HttpContext.User.Claims.FirstOrDefault(c => c.Type.Equals("Id"))?.Value, out int userId);
            if (result is false)
                throw new Exception("Unauthorized user");
            _userId = userId;
        }

    }
}
