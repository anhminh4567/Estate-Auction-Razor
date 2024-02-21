using Repository.Database;
using Repository.Database.Model.AuctionRelated;
using Service.Interfaces.Auction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Implementation.Auction
{
	public class AuctionReceiptRepository : BaseRepository<AuctionReceipt>, IAuctionReceiptRepository
	{
		public AuctionReceiptRepository(AuctionRealEstateDbContext context) : base(context)
		{
		}
	}
}
