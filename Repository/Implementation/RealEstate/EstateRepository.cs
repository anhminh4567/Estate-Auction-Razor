using Repository.Database;
using Repository.Database.Model.RealEstate;
using Repository.Implementation;
using Repository.Interfaces.RealEstate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Implementation.RealEstate
{
	public class EstateRepository : BaseRepository<Estate>, IEstateRepository
	{
		public EstateRepository(AuctionRealEstateDbContext context) : base(context)
		{
		}
	}
}
