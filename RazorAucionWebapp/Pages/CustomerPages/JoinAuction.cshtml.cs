using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Org.BouncyCastle.Asn1.X509;
using Repository.Database.Model.AppAccount;
using Repository.Database.Model.AuctionRelated;
using Repository.Database.Model.Enum;
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
		[BindProperty]
		public int AuctionId { get; set; }
		[BindProperty]
		public Auction Auction { get; set; } = default!;
		private int _userId { get; set; }
		private Account Account { get; set; }
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
				var getUser = (await _accountServices.GetById(_userId));
				var userBalance = getUser.Balance;
				var isBalanceEnough = (userBalance >= Auction.EntranceFee);
				if (isBalanceEnough)
				{
					if (Auction.Status.Equals(AuctionStatus.ONGOING) == false &&
						Auction.Status.Equals(AuctionStatus.NOT_STARTED) == false) 
					{
						ModelState.AddModelError(string.Empty, "auction is not happening");
						return Page();
					}
					if(Auction.JoinedAccounts.Count >= Auction.MaxParticipant)
					{
						ModelState.AddModelError(string.Empty, "reach max participant");
						return Page();
					}
					var isValidToJoin = await _joinedAuctionServices.CheckIfUserIsQualifiedToJoin(Account,Auction);
					if(isValidToJoin == false)
					{
						ModelState.AddModelError(string.Empty, "unqualified condition to join");
						return Page();
					}
					getUser.Balance -= Auction.EntranceFee;
					var updateResult = await _accountServices.Update(getUser);
					if (updateResult is false)
						return BadRequest();
					var newUserJoined = new JoinedAuction()
					{
						AccountId = _userId,
						AuctionId = Auction.AuctionId,
						RegisterDate = DateTime.Now,
						Status = Repository.Database.Model.Enum.JoinedAuctionStatus.REGISTERED
					};
					var createResult = await _joinedAuctionServices.Create(newUserJoined);
					if (createResult is null)
						return BadRequest("someting wrong wiith create JoinedAuction, try again");
					return RedirectToPage("../Index");
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
			Account = _accountServices.GetById(_userId).Result;
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
