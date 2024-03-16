using Repository.Database.Model.AppAccount;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Interfaces.AppAccount
{
	public interface IAccountImageRepository : ICrud<AccountImages>
	{
		Task<IList<AccountImages>> GetAllByAccountId(int id);
		Task<AccountImages> GetAsync(int AccountId);

    }
}
