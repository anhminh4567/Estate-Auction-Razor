using Repository.Database;
using Repository.Database.Model.AppAccount;
using Repository.Implementation;
using Repository.Interfaces.Auction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Implementation.Auction
{
	public class AuctionRepository : BaseRepository<Repository.Database.Model.AuctionRelated.Auction>, IAuctionRepository
	{
		public AuctionRepository(AuctionRealEstateDbContext context) : base(context)
		{
		}
	}
}
