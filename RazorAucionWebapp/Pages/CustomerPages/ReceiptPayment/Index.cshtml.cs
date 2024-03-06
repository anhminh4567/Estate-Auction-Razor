using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Repository.Database;
using Repository.Database.Model.AuctionRelated;
using Service.Services.Auction;

namespace RazorAucionWebapp.Pages.CustomerPages.ReceiptPayment
{
    public class IndexModel : PageModel
    {
        private readonly AuctionReceiptPaymentServices _auctionReceiptPaymentServices;
        private readonly AuctionReceiptServices _auctionReceiptServices;
        private readonly AuctionServices _auctionServices;

        public IndexModel(AuctionReceiptPaymentServices auctionReceiptPaymentServices, AuctionReceiptServices auctionReceiptServices, AuctionServices auctionServices)
        {
            _auctionReceiptPaymentServices = auctionReceiptPaymentServices;
            _auctionReceiptServices = auctionReceiptServices;
            _auctionServices = auctionServices;
        }

        public IList<AuctionReceiptPayment> AuctionReceiptPayment { get; set; } = default!;
        public AuctionReceipt AuctionReceipt { get; set; }

        public async Task<IActionResult> OnGetAsync(int? auctionId)
        {
            if (auctionId is null || auctionId == 0)
            {
                return NotFound();
            }
            try
            {
                await PopulateData(auctionId.Value);
                return Page();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return Page();
            }
        }
        private async Task PopulateData(int auctionId)
        {
            var tryGetReceipt = await _auctionReceiptServices.GetByAuctionId(auctionId, "Auction");
            if (tryGetReceipt == null)
            {
                ModelState.AddModelError(string.Empty, "No receipt can be found for this auction");
                throw new NullReferenceException();
            }
            else
                AuctionReceipt = tryGetReceipt;
            var tryGetReceiptTransaction = (await _auctionReceiptPaymentServices.GetByReceiptId(tryGetReceipt.ReceiptId, "Account")).ToList();
            if (tryGetReceiptTransaction == null)
            {
                ModelState.AddModelError(string.Empty, "No transaction can be found for this reeipt");
                throw new NullReferenceException();
            }
            else
                AuctionReceiptPayment = tryGetReceiptTransaction;
        }
    }
}
