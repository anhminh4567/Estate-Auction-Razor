using Microsoft.EntityFrameworkCore;
using Repository.Database;
using Repository.Database.Model.AppAccount;
using Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Implementation.AppAccount
{
    public class CompanyRepository : BaseRepository<Company>
    {
        public CompanyRepository(AuctionRealEstateDbContext context) : base(context)
        {
            
        }
    }
}
