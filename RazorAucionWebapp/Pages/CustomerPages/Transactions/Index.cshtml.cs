using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Repository.Database;
using Repository.Database.Model;

namespace RazorAucionWebapp.Pages.CustomerPages.Transactions
{
    public class IndexModel : PageModel
    {
        private readonly Repository.Database.AuctionRealEstateDbContext _context;

        public IndexModel(Repository.Database.AuctionRealEstateDbContext context)
        {
            _context = context;
        }

        public IList<Transaction> Transaction { get;set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.Transactions != null)
            {
                Transaction = await _context.Transactions
                .Include(t => t.Account).ToListAsync();
            }
        }
    }
}
