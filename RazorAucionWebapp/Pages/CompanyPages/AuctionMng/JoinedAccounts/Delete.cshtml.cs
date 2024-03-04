using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.EntityFrameworkCore;
using Repository.Database;
using Repository.Database.Model.AppAccount;
using Repository.Database.Model.AuctionRelated;
using Repository.Database.Model.Enum;
using Service.Services.AppAccount;
using Service.Services.Auction;

namespace RazorAucionWebapp.Pages.CompanyPages.AuctionMng.JoinedAccounts
{
    public class DeleteModel : PageModel
    {
        private readonly JoinedAuctionServices _joinedAuctionServices;
        private readonly AuctionServices _auctionServices;
        private readonly BidServices _bidServices;

        public DeleteModel(JoinedAuctionServices joinedAuctionServices, AuctionServices auctionServices, BidServices bidServices)
        {
            _joinedAuctionServices = joinedAuctionServices;
            _auctionServices = auctionServices;
            _bidServices = bidServices;
        }

        [BindProperty]
        public JoinedAuction JoinedAuction { get; set; } = default!;
        [BindProperty]
        public int AuctionId { get; set; }
        [BindProperty]
        public int AccountId { get; set; }
        public Auction Auction { get; set; }
        public async Task<IActionResult> OnGetAsync(int? auctionId, int? accountId)
        {
            if (auctionId is null || accountId is null)
                return NotFound();
			try
			{
                await PopulateData(accountId.Value,auctionId.Value);
                return Page();
			}
			catch (Exception ex)
			{
				return NotFound();
			}
		}

        public async Task<IActionResult> OnPostAsync()
		{
			try
			{
				await PopulateData(AccountId, AuctionId);
                var status = JoinedAuction.Status;
                if (status.Equals(JoinedAuctionStatus.REGISTERED))// only when user is registered can you ban him, if i quit, or alreay banned, then no
                {
                    JoinedAuction.Status = JoinedAuctionStatus.BANNED;
                    await _joinedAuctionServices.Update(JoinedAuction);// UPDate status to BANNED
                    var getBids = await _bidServices.GetByAuctionId_AccountId(AuctionId,AccountId);// REMOVE ALL BID OF THE USER IF EXIST
                    if(getBids.Count > 0)
                    {
                        await _bidServices.DeleteRange(getBids);
                    }
                    return RedirectToPage("./Index", new { auctionId = AuctionId});
                }
                else
                {
                    ModelState.AddModelError(string.Empty,"cannot banned this user because the status is "+ JoinedAuction.Status.ToString());
                    return Page();
                }
            }
			catch (Exception ex)
			{
                Console.WriteLine(ex.ToString());
				return NotFound();
			}
        }
        private async Task PopulateData(int accountId, int auctionId)
        {
            AuctionId = auctionId;
            AccountId = accountId;
            var tryGetJoinedAuction = (await _joinedAuctionServices.GetByAccountId_AuctionId(accountId: accountId, auctionId: auctionId));
            if(tryGetJoinedAuction is null) 
                throw new NullReferenceException(nameof(tryGetJoinedAuction));
            JoinedAuction = tryGetJoinedAuction;
            Auction = await _auctionServices.GetById(AuctionId);
        }

    }
}
