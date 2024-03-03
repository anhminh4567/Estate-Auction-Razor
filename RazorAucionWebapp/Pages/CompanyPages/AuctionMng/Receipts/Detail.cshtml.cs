using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Repository.Database;
using Repository.Database.Model.AuctionRelated;
using Service.Services.AppAccount;
using Service.Services.Auction;
using Service.Services.RealEstate;

namespace RazorAucionWebapp.Pages.CompanyPages.AuctionMng.Receipts
{
    public class DetailModel : PageModel
    {
        private readonly AuctionServices _auctionServices;
        private readonly EstateServices _estateServices;
        private readonly AccountServices _accountServices;
        private readonly AuctionReceiptPaymentServices _auctionReceiptPaymentServices;
        private readonly AuctionReceiptServices _auctionReceiptServices;
		public DetailModel(AuctionServices auctionServices, EstateServices estateServices, AccountServices accountServices, AuctionReceiptPaymentServices auctionReceiptPaymentServices, AuctionReceiptServices auctionReceiptServices)
		{
			_auctionServices = auctionServices;
			_estateServices = estateServices;
			_accountServices = accountServices;
			_auctionReceiptPaymentServices = auctionReceiptPaymentServices;
            _auctionReceiptServices = auctionReceiptServices;
		}

		public AuctionReceipt AuctionReceipt { get; set; } = default!; 
        public List<AuctionReceiptPayment> TransactionsForReceipt { get; set; }
        public async Task<IActionResult> OnGetAsync(int? auctionId)
        {
            if (auctionId == null)
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
            if(tryGetReceipt == null) 
            {
                ModelState.AddModelError(string.Empty, "No receipt can be found for this auction");
                throw new NullReferenceException();
            }
            else
				AuctionReceipt = tryGetReceipt;			
			var tryGetReceiptTransaction = await _auctionReceiptPaymentServices.GetByReceiptId(tryGetReceipt.ReceiptId, "Account");
			if (tryGetReceiptTransaction == null)
			{
				ModelState.AddModelError(string.Empty, "No transaction can be found for this reeipt");
				throw new NullReferenceException();
			}else
                TransactionsForReceipt = tryGetReceiptTransaction;   
		}
    }
}
