using Repository.Database.Model.AppAccount;
using Repository.Database;
using Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Service.Interfaces.AppAccount;

namespace Service.Implementation.AppAccount
{
    public class AccountImageRepository : BaseRepository<AccountImages>, IAccountImageRepository    
    {
        public AccountImageRepository(AuctionRealEstateDbContext context) : base(context)
        {
        }
    }
}
