﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Repository.Database;
using Repository.Database.Model.AppAccount;
using Repository.Database.Model;
using Repository.Database.Model.AuctionRelated;
using Repository.Database.Model.Enum;
using Repository.Database.Model.RealEstate;
using Service.MyHub.HubServices;
using Service.Services;
using Service.Services.Auction;
using Service.Services.RealEstate;
using Service.Services.AppAccount;

namespace RazorAucionWebapp.Pages.CompanyPages.AuctionMng
{
    public class DeleteModel : PageModel
    {
        private readonly AuctionServices _auctionServices;
        private readonly EstateServices _estateServices;
        private readonly BidServices _bidServices;
        private readonly NotificationServices _notificationServices;
        public DeleteModel(AuctionServices auctionServices, EstateServices estateServices, BidServices bidServices, NotificationServices notificationServices)
        {
            _auctionServices = auctionServices;
            _estateServices = estateServices;
            _bidServices = bidServices;
            _notificationServices = notificationServices;
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
                var result = await _auctionServices.CancelAuction(Auction);
                if (result.IsSuccess)
                {
                    await _notificationServices.SendMessage(Auction.AuctionId, NotificationType.CancelAuction);
                    return RedirectToPage("./Index");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, result.message);
                    return Page();
                }

            }
            catch (Exception ex)
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
                AuctionId = Auction.AuctionId;
                Estate = Auction.Estate;
            }

        }
    }
}
