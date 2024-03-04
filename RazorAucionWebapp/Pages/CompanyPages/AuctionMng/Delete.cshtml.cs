using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Repository.Database;
using Repository.Database.Model.AuctionRelated;
using Repository.Database.Model.Enum;
using Repository.Database.Model.RealEstate;
using Service.Services.Auction;
using Service.Services.RealEstate;

namespace RazorAucionWebapp.Pages.CompanyPages.AuctionMng
{
    public class DeleteModel : PageModel
    {
        private readonly AuctionServices _auctionServices;
        private readonly EstateServices _estateServices;
        private readonly BidServices _bidServices;
        public DeleteModel(AuctionServices auctionServices, EstateServices estateServices, BidServices bidServices)
        {
            _auctionServices = auctionServices;
            _estateServices = estateServices;
            _bidServices = bidServices;
        }

        [BindProperty]
        public Auction Auction { get; set; } = default!;
        [BindProperty]
        public int AuctionId { get; set; }
        public Estate Estate { get; set; }
        
        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            try
            {
                await PopulateData(id.Value);
            }
            catch (Exception ex) 
            {
                Console.WriteLine(ex.Message);
                return BadRequest(ex.Message);
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            try 
            {
                await PopulateData(AuctionId);
                var startTime = Auction.StartDate;
                var endTime = Auction.EndDate;
                var status = Auction.Status;
                if (DateTime.Compare(DateTime.Now,endTime) >= 0)
                {
                    ModelState.AddModelError(string.Empty, "over time to cancel");
                    return Page();
                }
                if(status.Equals(AuctionStatus.SUCCESS) ||
                   status.Equals(AuctionStatus.PENDING_PAYMENT) ||
                    status.Equals(AuctionStatus.CANCELLED))
                {
                    ModelState.AddModelError(string.Empty, "the status is not valid to cancel or you have already cancelled");
                    return Page();
                }
                Auction.Status = AuctionStatus.CANCELLED;
                ///
                /// Hoàn lại tiền Entrence Fee cho mọi nguời trong JoinedAuction có Status là REGISTERD
                var joinedAccounts = Auction.JoinedAccounts;
                await _auctionServices.Update(Auction);
                //foreach(var joinedAccount in joinedAccounts)
                //{
                //    joinedAccount.Status = JoinedAuctionStatus.QUIT;
                //}
                return RedirectToPage("./Index");
            }
            catch(Exception ex) 
            {
                Console.WriteLine(ex.Message);
                return BadRequest();
            }
            
        }
        private async Task PopulateData(int auctionId)
        {
            var auction = await _auctionServices.GetInclude(auctionId, "Estate,JoinedAccounts");
            if (auction is null)
            {
                throw new Exception("not found auction");
            }
            else
            {
                Auction = auction;
                AuctionId = Auction.AuctionId ;  
                Estate = Auction.Estate;
            }

        }
    }
}
