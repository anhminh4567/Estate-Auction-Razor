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

namespace RazorAucionWebapp.Pages.AdminPages.EstatesManage.AuctionsManage
{
    public class DeleteModel : PageModel
    {
        private readonly AuctionServices _auctionServices;

        public DeleteModel(AuctionServices auctionServices)
        {
            _auctionServices = auctionServices;
        }

        [BindProperty]
        public Auction Auction { get; set; } = default!;
    
        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
                return NotFound();
            try
            {
                await PopulateData(id.Value);
                return Page();
            }
            catch (Exception ex) 
            {
                return BadRequest();
            }

        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
                return NotFound();
            try
            {
                await PopulateData(id.Value);
                var result = await _auctionServices.AdminForceCancelAuction(Auction);
                if(result.IsSuccess) 
                {
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
                return BadRequest();
            }
        }
        private async Task PopulateData(int aucitonId) 
        {
            Auction = await _auctionServices.GetById(aucitonId) 
                ?? throw new Exception("cannot find auctoin with such id");

        }
    }
}
