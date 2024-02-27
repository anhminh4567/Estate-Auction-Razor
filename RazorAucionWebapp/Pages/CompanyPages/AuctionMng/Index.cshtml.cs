using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Repository.Database;
using Repository.Database.Model.AuctionRelated;
using Service.Services.AuctionService;

namespace RazorAucionWebapp.Pages.CompanyPages.AuctionMng
{
    public class IndexModel : PageModel
    {
        private readonly AuctionServices _auctionServices;

        public IndexModel(AuctionServices auctionServices)
        {
            _auctionServices = auctionServices;
        }
        public IList<Auction> Auctions { get;set; } = default!;
        private int _companyId { get; set; }
        public async Task OnGetAsync()
        {
            await PopulateData();   
        }
        private async Task PopulateData() 
        {
            var tryGetClaimId = int.TryParse(HttpContext.User.Claims.FirstOrDefault(c => c.Type.Equals("Id"))?.Value, out var companyId);
            if (tryGetClaimId is false)
                throw new Exception("claim id is not found, means this user is not legit or not exist at all");
            _companyId = companyId;
            Auctions = await _auctionServices.GetByCompanyId(_companyId);
        }
    }
}
