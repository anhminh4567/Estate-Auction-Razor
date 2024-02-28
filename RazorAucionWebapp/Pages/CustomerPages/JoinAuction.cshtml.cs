using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Org.BouncyCastle.Asn1.X509;
using Repository.Database.Model.AuctionRelated;
using Service.Services.AppAccount;
using Service.Services.Auction;

namespace RazorAucionWebapp.Pages.CustomerPages
{
    public class JoinAuctionModel : PageModel
    {
        private readonly JoinedAuctionServices _joinedAuctionServices;
        private readonly AuctionServices _auctionServices;
        public JoinAuctionModel(JoinedAuctionServices joinedAuctionServices)
        {
            _joinedAuctionServices = joinedAuctionServices;
        }
        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id is null) return RedirectToPage("./Index");
            GetUserId();
            await GetJoinAuction(id.Value);
            return Page();
        }
        private int UserId { get; set; }
        [BindProperty]
        public Auction Auction { get; set; } = default!;
        private void GetUserId()
        {
            var result = int.TryParse(HttpContext.User.Claims.FirstOrDefault(c => c.Type.Equals("Id"))?.Value, out int userId);
            if (result is false) throw new Exception("Unauthorized user");
            UserId = userId;
        }
        private async Task GetJoinAuction(int id)
        {
            Auction = await _auctionServices.GetById(id);
        }
    }
}
