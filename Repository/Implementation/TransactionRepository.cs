using Microsoft.EntityFrameworkCore;
using Repository.Database;
using Repository.Database.Model;
using Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Implementation
{
	public class TransactionRepository : BaseRepository<Transaction>, ITransactionRepository
	{
		public TransactionRepository(AuctionRealEstateDbContext context) : base(context)
		{
		}

		public async Task<Transaction?> GetFullAsync(int id)
		{
			return await _set.Include(t => t.Account)
				.FirstOrDefaultAsync(t => t.TransactionId == id);
		}
	}
}
