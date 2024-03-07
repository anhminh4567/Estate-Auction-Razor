using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using MailKit.Net.Imap;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Repository.Database;
using Repository.Database.Model.AuctionRelated;
using Repository.Database.Model.Enum;
using Repository.Database.Model.RealEstate;
using Service.Services.AppAccount;
using Service.Services.Auction;
using Service.Services.RealEstate;

namespace RazorAucionWebapp.Pages.CompanyPages.EstateMng
{
    public class DeleteModel : PageModel
    {
        private readonly EstateServices _estateServices;
        private readonly AuctionServices _auctionServices;
        private readonly CompanyServices _companyServices;

        public DeleteModel(EstateServices estateServices, AuctionServices auctionServices, CompanyServices companyServices)
        {
            _estateServices = estateServices;
            _auctionServices = auctionServices;
            _companyServices = companyServices;
        }

        [BindProperty]
        public Estate Estate { get; set; } = default!;
        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var estate = await _estateServices.GetIncludes(id.Value, "Auctions");
            if (estate is null)
            {
                return NotFound();
            }
            else
            {
                Estate = estate;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            try
            {
                var getEstate = await _estateServices.GetIncludes(id.Value, "Auctions");
                Estate = getEstate;
                //var isDeletable = true;
                //if(getEstate.Status.Equals(EstateStatus.REMOVED) || getEstate.Status.Equals(EstateStatus.BANNDED) || getEstate.Status.Equals(EstateStatus.FINISHED))
                //{
                //    ModelState.AddModelError(string.Empty, "cannot delete, estate is already " + getEstate.Status.ToString());
                //    return Page();
                //}
                //if (getEstate.Auctions is not null)
                //{
                //    foreach (var auction in getEstate.Auctions)
                //    {
                //        if(auction.Status.Equals(AuctionStatus.SUCCESS) ||
                //            auction.Status.Equals(AuctionStatus.ONGOING) ||
                //            auction.Status.Equals(AuctionStatus.PENDING_PAYMENT))
                //        {
                //            isDeletable = false;
                //            ModelState.AddModelError(string.Empty, "cannot delete, an auction is " +  auction.Status.ToString());
                //            break;
                //        }
                //    }
                //}
                //if (isDeletable)
                //{
                //    var result = await _estateServices.Delete(getEstate);
                //    if (result is false)
                //        throw new Exception("something wrong when delete, it result in false");
                //    return RedirectToPage("./Index");
                //}
                var deleteEstate = await _estateServices.DeleteEstate(Estate);
                if (deleteEstate.IsSuccess)
                {
                    return RedirectToPage("./Index");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, deleteEstate.message);
                    return Page();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return NotFound();
            }
        }
    }
}
