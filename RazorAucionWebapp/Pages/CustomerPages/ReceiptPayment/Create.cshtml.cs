using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Build.Framework;
using Repository.Database;
using Repository.Database.Model.AppAccount;
using Repository.Database.Model.AuctionRelated;
using Repository.Database.Model.Enum;
using Repository.Database.Model.RealEstate;
using Service.Services.AppAccount;
using Service.Services.Auction;
using Service.Services.RealEstate;

namespace RazorAucionWebapp.Pages.CustomerPages.ReceiptPayment
{
    public class CreateModel : PageModel
    {
        private readonly AuctionReceiptPaymentServices _auctionReceiptPaymentServices;
        private readonly AuctionReceiptServices _auctionReceiptServices;
        private readonly AuctionServices _auctionServices;
        private readonly AccountServices _accountServices;
        private readonly EstateServices _estateService;

        public CreateModel(AuctionReceiptPaymentServices auctionReceiptPaymentServices, AuctionReceiptServices auctionReceiptServices, AuctionServices auctionServices, AccountServices accountServices, EstateServices estateService)
        {
            _auctionReceiptPaymentServices = auctionReceiptPaymentServices;
            _auctionReceiptServices = auctionReceiptServices;
            _auctionServices = auctionServices;
            _accountServices = accountServices;
            _estateService = estateService;
        }

        public async Task<IActionResult> OnGetAsync(int? receiptId)
        {
            if(receiptId is null)
            {
                return NotFound();
            }
            try
            {
                await PopulateData(receiptId.Value);
                return Page();
            }catch (Exception ex) 
            {
                Console.WriteLine(ex.Message);
                return NotFound();
            }
        }

        [BindProperty]
        [Required]
        public decimal PayAmount { get; set; }
        [BindProperty]
        [Required]
        public int ReceiptId { get; set; }
        public IList<AuctionReceiptPayment> AuctionReceiptPayment { get; set; } = default!;
        public AuctionReceipt AuctionReceipt { get; set; }
        public Account UserAccount { get; set; }
        public Auction Auction { get; set; }
        public Estate Estate { get; set; }
        public Account Company { get; set; }
        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            try 
            {
                await PopulateData(ReceiptId);

                //LOGIC START
                var userBalance = UserAccount.Balance;
                var moneyToPayRemain = AuctionReceipt.RemainAmount;
                var payEndDate = Auction.EndPayDate;
                if(Auction.Status.Equals(AuctionStatus.FAILED_TO_PAY)|| Auction.Status.Equals(AuctionStatus.SUCCESS)) 
                {
					ModelState.AddModelError(string.Empty, "Auction is finished");
					return Page();
				}
                if (PayAmount > userBalance) 
                {
                    ModelState.AddModelError(string.Empty, "You dont have enough balancee" );
                    return Page();
                }
                if (PayAmount <= 0 || PayAmount > moneyToPayRemain) // if the money you are paying is moree than you need to pay
                {
                    ModelState.AddModelError(string.Empty,"your money must > 0 and <= "+ moneyToPayRemain.ToString());
                    return Page();
                }
                if (DateTime.Compare(DateTime.Now, payEndDate) >= 0)
                {
                    ModelState.AddModelError(string.Empty, "Your pay Time is over");
                    return Page();
                }
                var newPayment = new AuctionReceiptPayment()
                {
                    AccountId = UserAccount.AccountId,
                    ReceiptId = AuctionReceipt.ReceiptId,
                    PayAmount = PayAmount,
                    PayTime = DateTime.Now,
                };
                /////////// CREATE ///////////
                var createResult = await _auctionReceiptPaymentServices.Create(newPayment);
                if(createResult is null) 
                {
                    ModelState.AddModelError(string.Empty,"cannot create right now, try again later");
                    throw new Exception();
                }
                /////////// UPDATE OLD RECEIPT ///////////
                AuctionReceipt.RemainAmount -= createResult.PayAmount;// update the receipt remain to pay
                var updateReceiptResult = await _auctionReceiptServices.Update(AuctionReceipt);
                if(updateReceiptResult is false) 
                {
                    ModelState.AddModelError(string.Empty,"fail to update receipt try again later");
                    //await _auctionReceiptPaymentServices.Delete(createResult);
                    throw new Exception();
                }
                /////////// UPDATE BALANCE///////////
                UserAccount.Balance -= createResult.PayAmount;// update the receipt remain to pay
                var updateBalanceResult = await _accountServices.Update(UserAccount);
                if (updateBalanceResult is false)
                {
                    ModelState.AddModelError(string.Empty, "fail to update balance try again later");
                    //await _auctionReceiptPaymentServices.Delete(createResult);
                    throw new Exception();
                }
                /////////// UPDATE Company BALANCE///////////
                Company.Balance += createResult.PayAmount;
                await _accountServices.Update(Company);
				/////////// UPDATE If NO REMAIN AMOUNT///////////
				if (AuctionReceipt.RemainAmount == 0)
                {
                    Auction.Status = Repository.Database.Model.Enum.AuctionStatus.SUCCESS;
                    Estate.Status = Repository.Database.Model.Enum.EstateStatus.FINISHED;
                    await _auctionServices.Update(Auction);
                    await _estateService.Update(Estate);
                }

                return RedirectToPage("./Index",new {auctionId = Auction.AuctionId});
            }
            catch (Exception ex) 
            {
                Console.WriteLine(ex.Message);
                return Page();
            }
        }
        private async Task PopulateData(int receiptId)
        {
            var tryGetReceipt =  await _auctionReceiptServices.GetById(receiptId, "Payments,Buyer,Auction.Estate.Company");
            if(tryGetReceipt is null) 
                throw new ArgumentNullException(nameof(tryGetReceipt));
            AuctionReceipt = tryGetReceipt;
            ReceiptId = receiptId;
            Auction = AuctionReceipt.Auction;
            Estate = Auction.Estate;
            Company = Estate.Company;
            UserAccount = AuctionReceipt.Buyer;
            AuctionReceiptPayment = AuctionReceipt.Payments;
        }
    }
}
