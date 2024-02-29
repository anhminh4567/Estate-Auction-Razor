using Repository.Database;
using Repository.Database.Model.AuctionRelated;
using Repository.Interfaces.Auction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Implementation.Auction
{
	public class AuctionReceiptPaymentRepository : BaseRepository<AuctionReceiptPayment>, IAuctionReceiptPaymentRepository
	{
		public AuctionReceiptPaymentRepository(AuctionRealEstateDbContext context) : base(context)
		{
		}
	}
}
