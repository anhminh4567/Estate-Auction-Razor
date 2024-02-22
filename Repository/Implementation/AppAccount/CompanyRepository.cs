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
	public class CompanyRepository : BaseRepository<Company>, ICompanyRepository
    {
        public CompanyRepository(AuctionRealEstateDbContext context) : base(context)
        {
            
        }
    }
}
