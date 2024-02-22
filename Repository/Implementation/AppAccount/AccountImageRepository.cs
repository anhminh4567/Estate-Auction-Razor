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
	}
}
