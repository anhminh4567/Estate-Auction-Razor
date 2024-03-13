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

namespace RazorAucionWebapp.Pages.AdminPages.Revenue
{
    public class IndexModel : PageModel
    {
        private readonly Repository.Database.AuctionRealEstateDbContext _context;
        private readonly AuctionReceiptServices _auctionReceiptServices;
        private readonly AuctionReceiptPaymentServices _auctionReceiptPaymentServices;

        public IndexModel(Repository.Database.AuctionRealEstateDbContext context, AuctionReceiptPaymentServices auctionReceiptPaymentServices, AuctionReceiptServices auctionReceiptServices)
        {
            _context = context;
            _auctionReceiptPaymentServices = auctionReceiptPaymentServices;
            _auctionReceiptServices = auctionReceiptServices;
        }
        [BindProperty]
        public IList<AuctionReceipt> AuctionReceipt { get;set; } = default!;

        [BindProperty]
        public decimal TotalCommission { get;set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.AuctionReceipts != null)
            {
                //AuctionReceipt = await _context.AuctionReceipts
                //.Include(a => a.Auction)
                //.Include(a => a.Buyer).ToListAsync();
                AuctionReceipt = await _auctionReceiptServices.GetAll();


                foreach (var receipt in AuctionReceipt)
                {
                    TotalCommission += receipt.Commission;
                }
            }
        }
    }
}
