using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Org.BouncyCastle.Asn1.X509;
using Repository.Database.Model;
using Repository.Database.Model.AppAccount;
using Repository.Database.Model.AuctionRelated;
using Repository.Database.Model.Enum;
using Service.MyHub.HubServices;
using Service.Services;
using Service.Services.AppAccount;
using Service.Services.Auction;

namespace RazorAucionWebapp.Pages.CustomerPages
{
	public class JoinAuctionModel : PageModel
	{
		private readonly JoinedAuctionServices _joinedAuctionServices;
		private readonly AuctionServices _auctionServices;
		private readonly AccountServices _accountServices;
		private readonly BidServices _bidServices;
        private readonly NotificationServices _notificationServices;
        public JoinAuctionModel(JoinedAuctionServices joinedAuctionServices, AuctionServices auctionServices, AccountServices accountServices, BidServices bidServices, NotificationServices notificationServices)
		{
			_joinedAuctionServices = joinedAuctionServices;
			_auctionServices = auctionServices;
			_accountServices = accountServices;
			_bidServices = bidServices;
            _notificationServices = notificationServices;
        }
        [BindProperty]
        public int AuctionId { get; set; }
        [BindProperty]
        public Auction Auction { get; set; } = default!;
        private int _userId { get; set; }
        private Account Account { get; set; }
        public JoinedAuction? JoinedAuction { get; set; }
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
                var result = await _joinedAuctionServices.JoinAuction(Account.AccountId, Auction.AuctionId);
                if (result.IsSuccess)
                {
                    await GetJoinAuction(AuctionId);
                    await _notificationServices.SendMessage(_userId, Auction.AuctionId, NotificationType.JoinAuction);
                    return RedirectToPage("../Index");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, result.message);
                    return Page();
                }
                //var getUser = (await _accountServices.GetById(_userId));
                //var userBalance = getUser.Balance;
                //var isBalanceEnough = (userBalance >= Auction.EntranceFee);
                //if (isBalanceEnough)
                //{
                //	if (//Auction.Status.Equals(AuctionStatus.ONGOING) == false &&
                //		Auction.Status.Equals(AuctionStatus.NOT_STARTED) == false)
                //	{
                //		ModelState.AddModelError(string.Empty, "auction is happening");
                //		return Page();
                //	}
                //	if (Auction.JoinedAccounts.Count >= Auction.MaxParticipant)
                //	{
                //		ModelState.AddModelError(string.Empty, "reach max participant");
                //		return Page();
                //	}
                //	var isValidToJoin = await _joinedAuctionServices.CheckIfUserIsQualifiedToJoin(Account, Auction);
                //	if (isValidToJoin == false)
                //	{
                //		ModelState.AddModelError(string.Empty, "unqualified condition to join");
                //		return Page();
                //	}
                //	getUser.Balance -= Auction.EntranceFee;
                //	// Chưa có chuyênr tiền vào company, mới trừ tiền của customer 
                //	var updateResult = await _accountServices.Update(getUser);
                //	if (updateResult is false)
                //		return BadRequest();
                //	var newUserJoined = new JoinedAuction()
                //	{
                //		AccountId = _userId,
                //		AuctionId = Auction.AuctionId,
                //		RegisterDate = DateTime.Now,
                //		Status = Repository.Database.Model.Enum.JoinedAuctionStatus.REGISTERED
                //	};
                //	var createResult = await _joinedAuctionServices.Create(newUserJoined);
                //	if (createResult is null)
                //		return BadRequest("someting wrong wiith create JoinedAuction, try again");
                //	return RedirectToPage("../Index");
                //}

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest();
            }
        }
        /// <summary>
        /// ON QUITTING THE AUCTION
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> OnPostQuitAsync()
        {
            try
            {
                GetUserId();
                await GetJoinAuction(AuctionId);
                var result = await _joinedAuctionServices.QuitAuction(Account.AccountId, Auction.AuctionId);
                if (result.IsSuccess)
                {
                    await _notificationServices.SendMessage(_userId, Auction.AuctionId, NotificationType.QuitAuction);
                    return RedirectToPage("../Index");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, result.message);
                    return Page();
                }
                //var getUser = (await _accountServices.GetById(_userId));
                //var getAuction = await _auctionServices.GetById(JoinedAuction.AuctionId.Value);
                //if (Auction.Status.Equals(AuctionStatus.NOT_STARTED)  || Auction.Status.Equals(AuctionStatus.ONGOING) )
                //{
                //	if (JoinedAuction.Status.Equals(JoinedAuctionStatus.REGISTERED))
                //	{
                //		var getBids = await _bidServices.GetByAuctionId_AccountId(AuctionId, _userId);// REMOVE ALL BID OF THE USER IF EXIST
                //		if (getBids is not null && getBids.Count > 0)
                //		{
                //			await _bidServices.DeleteRange(getBids);
                //		}
                //		await _joinedAuctionServices.DeleteRange(new List<JoinedAuction>() { JoinedAuction });
                //		getUser.Balance += getAuction.EntranceFee;
                //		await _accountServices.Update(getUser);
                //	}
                //	else
                //	{
                //		ModelState.AddModelError(string.Empty, "cannot delete, status not valid");
                //		return Page();
                //	}
                //}
                //else
                //{
                //	ModelState.AddModelError(string.Empty, "auction is finished");
                //	return Page();
                //}


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
            Account = _accountServices.GetById(_userId).Result;
        }
        private async Task GetJoinAuction(int auctionId)
        {
            var tryGetJoinedAuction = await _auctionServices.GetInclude(auctionId, "JoinedAccounts.Account,Estate.Company");
            if (tryGetJoinedAuction is null)
                throw new Exception("cannot find auction with this id");
            Auction = tryGetJoinedAuction;
            JoinedAuction = await _joinedAuctionServices.GetByAccountId_AuctionId(_userId, auctionId);
        }
    }
}
