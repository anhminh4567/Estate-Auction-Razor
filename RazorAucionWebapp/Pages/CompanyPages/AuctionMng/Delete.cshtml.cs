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
using Service.Services.RealEstate;

namespace RazorAucionWebapp.Pages.CompanyPages.AuctionMng
{
    public class DeleteModel : PageModel
    {
        private readonly AuctionServices _auctionServices;
        private readonly EstateServices _estateServices;

        public DeleteModel(AuctionServices auctionServices, EstateServices estateServices)
        {
            _auctionServices = auctionServices;
            _estateServices = estateServices;
        }

        [BindProperty]
        public Auction Auction { get; set; } = default!;
        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            try
            {
                var auction = await _auctionServices.GetById(id.Value);
                if (auction is null)
                {
                    return NotFound();
                }
                else
                {
                    Auction = auction;
                }
            }
            catch (Exception ex) 
            {
                Console.WriteLine(ex.Message);
                return BadRequest(ex.Message);
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id is null)
            {
                return NotFound();
            }
            try 
            {
                var auction = await _auctionServices.GetById(id.Value);
                if (auction is null)
                {
                    return NotFound();
                }
                Auction = auction;
                var result = await _auctionServices.Delete(Auction);
                if (result is false)
                    throw new Exception("cannot set new status for this one, something wrong with it");
                return RedirectToPage("./Index");
            }
            catch(Exception ex) 
            {
                Console.WriteLine(ex.Message);
                return BadRequest();
            }
            
        }
    }
}
