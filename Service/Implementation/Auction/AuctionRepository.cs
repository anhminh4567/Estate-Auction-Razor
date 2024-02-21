using Repository.Database;
using Repository.Database.Model.AppAccount;
using Service.Interfaces.Auction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Implementation.Auction
{
	public class AuctionRepository : BaseRepository<Repository.Database.Model.AuctionRelated.Auction>, IAuctionRepository
	{
		public AuctionRepository(AuctionRealEstateDbContext context) : base(context)
		{
		}
	}
}
