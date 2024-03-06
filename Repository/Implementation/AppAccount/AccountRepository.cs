using Microsoft.EntityFrameworkCore;
using Repository.Database;
using Repository.Database.Model.AppAccount;
using Repository.Implementation;
using Repository.Interfaces.AppAccount;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Implementation.AppAccount
{
	public class AccountRepository : BaseRepository<Account>, IAccountRepository
    {
        public AccountRepository(AuctionRealEstateDbContext context) : base(context)
        {
        }

		public async Task<Account?> GetByEmail(string email, bool isCaseSensitive)
		{
            return isCaseSensitive ? await _set.FirstOrDefaultAsync(a => a.Email.Equals(email,StringComparison.OrdinalIgnoreCase))
                : await _set.FirstOrDefaultAsync(a => a.Email.Equals(email));
		}

		public async Task<Account> GetByEmailPassword(string email, string password)
        {
            var getAccount = await _set.FirstOrDefaultAsync(a => a.Email == email && a.Password == password);
            return getAccount;
        }

		public async Task<Account?> GetFullAsync(int accountId)
		{
			return await _set
				.Include(a => a.AccountImages).ThenInclude(img => img.Image)
				.Include(a => a.Transactions).FirstOrDefaultAsync(a => a.AccountId == accountId);
		}

		public async Task<List<Account>> GetActiveCustomers()
        {
			return await _set.Where(p => p.Role == Repository.Database.Model.Enum.Role.CUSTOMER && p.Status == Repository.Database.Model.Enum.AccountStatus.ACTIVED).ToListAsync();
        }
	}
}
