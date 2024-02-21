using Repository.Database.Model.AppAccount;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Interfaces
{
	public interface IAccountRepository : ICrud<Account>
	{
		Task<Account> GetByEmailPassword(string email, string password);
	}
}
