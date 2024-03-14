using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Repository.Database;
using Repository.Database.Model.AppAccount;
using Repository.Database.Model.AuctionRelated;
using Repository.Database.Model.Enum;
using Service.Services.AppAccount;
using Service.Services.Auction;
using Service.Services.RealEstate;

namespace RazorAucionWebapp.Pages.CompanyPages.AuctionMng
{
    public class UpdateModel : PageModel
    {
        private readonly AuctionServices _auctionServices;
        private readonly EstateServices _estateServices;
        private readonly JoinedAuctionServices _joinedAuctionServices;
        private readonly BidServices _bidServices;

        public UpdateModel(AuctionServices auctionServices, EstateServices estateServices, JoinedAuctionServices joinedAuctionServices, BidServices bidServices)
        {
            _auctionServices = auctionServices;
            _estateServices = estateServices;
            _joinedAuctionServices = joinedAuctionServices;
            _bidServices = bidServices;
        }

        [BindProperty]
        public bool IsCancelled { get; set; }

        public Auction Auction { get; set; } = default!;
        public AuctionReceipt AuctionReceipt { get; set; }
        public List<Account?> AllJoinedAccount { get; set; }// total Joined
        public List<Account?> JoinedAccounts { get; set; }// those who haven't quit or banned 
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
                return BadRequest();
            }
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            try
            {
                await PopulateData(Auction.AuctionId);
                var maxParticipant = Auction.MaxParticipant;
                var status = Auction.Status;
            }
            catch (DbUpdateConcurrencyException ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest();
            }


            return RedirectToPage("./Index");
        }

        private async Task PopulateData(int id)
        {
            var tryGetAuction = await _auctionServices.GetInclude(id, "Estate,JoinedAccounts.Account,AuctionReceipt");
            if (tryGetAuction == null)
                throw new NullReferenceException();
            Auction = tryGetAuction;
            AllJoinedAccount = Auction.JoinedAccounts?
                .Select(j => j.Account).ToList();
            JoinedAccounts = Auction.JoinedAccounts?
                .Where(j => j.Status.Equals(JoinedAuctionStatus.REGISTERED))
                .Select(j => j.Account).ToList();
        }
    }
}
