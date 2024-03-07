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
    public class IndexModel : PageModel
    {
        private readonly AuctionServices _auctionServices;

        public IndexModel(AuctionServices auctionServices)
        {
            _auctionServices = auctionServices;
        }

        public List<Auction> Auctions { get;set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? estateId)
        {
            if (estateId == null)
                return BadRequest();
            try
            {
                await PopulateData(estateId.Value);
                return Page();
            }catch (Exception ex) 
            {
                return BadRequest();
            }
        }
        private async Task PopulateData(int estateId)
        {
            Auctions = await _auctionServices.GetByEstateId(estateId);
            if (Auctions == null)
                Auctions = new List<Auction>();
        }
    }
}
