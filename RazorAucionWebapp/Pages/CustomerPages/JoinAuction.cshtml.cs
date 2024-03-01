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
        private readonly AccountServices _accountServices;
		public JoinAuctionModel(JoinedAuctionServices joinedAuctionServices, AuctionServices auctionServices, AccountServices accountServices)
		{
			_joinedAuctionServices = joinedAuctionServices;
			_auctionServices = auctionServices;
			_accountServices = accountServices;
		}
		private int _userId { get; set; }
        [BindProperty]
        public int AuctionId { get; set; }
		[BindProperty]
		public Auction Auction { get; set; } = default!;
		public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id is null) 
                return RedirectToPage("./Index");
            try
            {
				GetUserId();
				await GetJoinAuction(id.Value);
			}
			catch (Exception ex) 
            {
                Console.WriteLine(ex.Message);
                return BadRequest();
            }
            return Page();
        }
		public async Task<IActionResult> OnPostAsync()
		{
            try
            {
				GetUserId();
				await GetJoinAuction(AuctionId);
                var getUserBalance = (await _accountServices.GetById(_userId)).Balance;
                var isBalanceEnough = (getUserBalance >= Auction.EntranceFee); 
                if(isBalanceEnough) 
                {
                    // extract that amount from balance
                    return Page();
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "not enough balance, put some money in account");
                    return Page();
                }
            }
			catch (Exception ex) 
            {
				Console.WriteLine(ex.Message);
				return BadRequest();
			}
			return Page();
		}

        private void GetUserId()
        {
            var result = int.TryParse(HttpContext.User.Claims.FirstOrDefault(c => c.Type.Equals("Id"))?.Value, out int userId);
            if (result is false) 
                throw new Exception("Unauthorized user");
            _userId = userId;
        }
        private async Task GetJoinAuction(int id)
        {
            var tryGetJoinedAuction = await _auctionServices.GetInclude(id, "JoinedAccounts.Account");
            if (tryGetJoinedAuction is null)
                throw new Exception("cannot find auction with this id");
            Auction = tryGetJoinedAuction;

		}
    }
}
