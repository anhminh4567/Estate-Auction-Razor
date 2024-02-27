using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Repository.Database.Model.AuctionRelated;
using Service.Services.AuctionService;

namespace RazorAucionWebapp.Pages
{
    public class IndexModel : PageModel
    {
        private readonly AuctionServices _auctionServices;

        public IndexModel(AuctionServices auctionServices)
        {
            _auctionServices = auctionServices;
        }
        public List<Auction> Auctions { get; set; }
        //frontend lo 2 so nay, dung paging
        [BindProperty]
        public int PageStart { get; set; }
        [BindProperty]
        public int DisplayAmount { get; set; }
        // PageStart: la vi tri bat dau trong database
        // DisplayAmount: la lay bao nhieu tu vi tri do
        public async Task<IActionResult> OnGetAsync()
        {
            // 2 so nay la static, mot frontend sua lai, bind 2 so nay 
            PageStart = 0;
            DisplayAmount = 2;
            // 2 so nay la static, mot frontend sua lai, bind 2 so nay 
            int correctStartValue = PageStart * 10;
            await PopulateData(correctStartValue, DisplayAmount);
            return Page();
        }
        private async Task PopulateData(int start, int amount ) 
        {
            Auctions = await _auctionServices.GetRangeIncludeEstate(start,amount);
        }
    }
}