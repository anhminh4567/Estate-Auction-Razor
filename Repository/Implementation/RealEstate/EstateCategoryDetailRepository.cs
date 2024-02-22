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
	public class EstateCategoryDetailRepository : BaseRepository<EstateCategoryDetail>, IEstateCategoryDetailRepository
	{
		public EstateCategoryDetailRepository(AuctionRealEstateDbContext context) : base(context)
		{
		}
	}
}
