using Repository.Database.Model.AppAccount;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Interfaces.AppAccount
{
	public interface ICompanyRepository : ICrud<Company>
	{
		//Task<Company?> GetFullAsync(int accountId);
	}
}
