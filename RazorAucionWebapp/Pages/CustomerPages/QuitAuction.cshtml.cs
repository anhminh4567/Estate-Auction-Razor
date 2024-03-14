using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Repository.Database.Model;
using Repository.Database.Model.AppAccount;
using Repository.Database.Model.AuctionRelated;
using Repository.Database.Model.Enum;
using Service.MyHub.HubServices;
using Service.Services;
using Service.Services.AppAccount;
using Service.Services.Auction;
using System.Security.Claims;

namespace RazorAucionWebapp.Pages.CustomerPages
{
    public class QuitAuctionModel : PageModel
    {
        private readonly JoinedAuctionServices _joinedAuctionServices;
        private readonly NotificationServices _notificationServices;
        public QuitAuctionModel(JoinedAuctionServices joinedAuctionServices, NotificationServices notificationServices)
        {
            _joinedAuctionServices = joinedAuctionServices;
            _notificationServices = notificationServices;
        }
        private int _userId { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            try
            {
                GetUserId();
                var result = await _joinedAuctionServices.QuitAuction(_userId, id.Value);
                if (result.IsSuccess)
                {
                    await _notificationServices.SendMessage(_userId, id.Value, NotificationType.QuitAuction);
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
        private void GetUserId()
        {
            var result = int.TryParse(HttpContext.User.Claims.FirstOrDefault(c => c.Type.Equals("Id"))?.Value, out int userId);
            if (result is false)
                throw new Exception("Unauthorized user");
            _userId = userId;
        }

    }
}
