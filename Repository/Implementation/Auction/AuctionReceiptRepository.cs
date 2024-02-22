using Repository.Database;
using Repository.Database.Model.AuctionRelated;
using Repository.Implementation;
using Repository.Interfaces.Auction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Implementation.Auction
{
	public class AuctionReceiptRepository : BaseRepository<AuctionReceipt>, IAuctionReceiptRepository
	{
		public AuctionReceiptRepository(AuctionRealEstateDbContext context) : base(context)
		{
		}
	}
}
