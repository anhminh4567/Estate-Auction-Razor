using Microsoft.EntityFrameworkCore;
using Repository.Database;
using Repository.Database.Model.AppAccount;
using Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Implementation
{
	public class AccountRepository : IAccountRepository
	{
		private readonly AuctionRealEstateDbContext _context;
		public AccountRepository(AuctionRealEstateDbContext context)
		{
			this._context = context;
		}

		public async Task<List<Account>> GetAllAsync()
		{
			return await _context.Accounts.ToListAsync();
		}

		public async Task<Account> GetAsync(int id)
		{
			return await _context.Accounts.FirstOrDefaultAsync(c => c.AccountId == id);
		}
		public async Task<Account> CreateAsync(Account t)
		{
			throw new NotImplementedException();
		}

		public async Task<bool> DeleteAsync(int id)
		{
			throw new NotImplementedException();
		}
		public async Task<bool> UpdateAsync(Account entity)
		{
			throw new NotImplementedException();
		}
	}
}
