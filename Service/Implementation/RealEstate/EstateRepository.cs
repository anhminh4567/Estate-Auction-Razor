using Repository.Database;
using Repository.Database.Model.RealEstate;
using Service.Interfaces.RealEstate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Implementation.RealEstate
{
	public class EstateRepository : BaseRepository<Estate>, IEstateRepository
	{
		public EstateRepository(AuctionRealEstateDbContext context) : base(context)
		{
		}
	}
}
