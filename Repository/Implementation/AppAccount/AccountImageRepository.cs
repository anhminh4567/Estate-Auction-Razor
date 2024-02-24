using Repository.Database.Model.AppAccount;
using Repository.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Repository.Interfaces.AppAccount;

namespace Repository.Implementation.AppAccount
{
	public class AccountImageRepository : BaseRepository<AccountImages>, IAccountImageRepository
	{
		public AccountImageRepository(AuctionRealEstateDbContext context) : base(context)
		{
		}

		public override async Task<AccountImages> GetAsync(int AccountId)
		{
			return await _set.FirstOrDefaultAsync(img => img.AccountId == AccountId);
		}

		public async Task<IList<AccountImages>> GetAllByAccountId(int AccountId)
		{
			return await _set.Where(img => img.AccountId.Equals(AccountId)).ToListAsync();
		}
	}
}
