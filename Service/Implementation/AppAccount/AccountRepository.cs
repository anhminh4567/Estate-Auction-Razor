using Microsoft.EntityFrameworkCore;
using Repository.Database;
using Repository.Database.Model.AppAccount;
using Service.Interfaces;
using Service.Interfaces.AppAccount;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Implementation.AppAccount
{
    public class AccountRepository : BaseRepository<Account>, IAccountRepository
    {
        public AccountRepository(AuctionRealEstateDbContext context) : base(context)
        {
        }
        public async Task<Account> GetByEmailPassword(string email, string password)
        {
            var getAccount = await _set.FirstOrDefaultAsync(a => a.Email == email && a.Password == password);
            return getAccount;
        }
    }
}
