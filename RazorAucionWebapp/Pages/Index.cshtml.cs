using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Repository.Database.Model.AuctionRelated;
using Service.Services.Auction;

namespace RazorAucionWebapp.Pages
{
    public class Item
    {
        public Auction Auction { get; set; }
        public bool isJoined { get; set; }

    }
    public class IndexModel : PageModel
    {
        private readonly AuctionServices _auctionServices;

        public IndexModel(AuctionServices auctionServices)
        {
            _auctionServices = auctionServices;
        }
        public List<Item> Auctions { get; set; }
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
            DisplayAmount = 3;
            // 2 so nay la static, mot frontend sua lai, bind 2 so nay 
            int correctStartValue = PageStart * 10;
            GetUserId();
            await PopulateData(correctStartValue, DisplayAmount);
            return Page();
        }
        private int UserId { get; set; }
        private void GetUserId()
        {
            var result = int.TryParse(HttpContext.User.Claims.FirstOrDefault(c => c.Type.Equals("Id"))?.Value, out var userId);
            UserId = userId;
        }
        private async Task PopulateData(int start, int amount ) 
        {
            var list = await _auctionServices.GetRangeInclude_Estate_Company(start,amount);
            foreach(var i in list)
            {
                Auctions.Add(new Item()
                {
                    Auction = i,
                    isJoined = await _auctionServices.CheckForJoinedAuction(UserId, i.AuctionId)
                });
            }
        }
    }
}