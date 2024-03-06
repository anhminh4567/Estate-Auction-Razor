using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Repository.Database.Model.AppAccount;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Interfaces.AppAccount
{
	public interface IAccountRepository : ICrud<Account>
	{
		Task<Account> GetByEmailPassword(string email, string password);
		Task<Account> GetByEmail(string email, bool isCaseSensitive);
		Task<Account> GetFullAsync(int accountId);
		Task<List<Account>> GetActiveCustomers();
    }
}
