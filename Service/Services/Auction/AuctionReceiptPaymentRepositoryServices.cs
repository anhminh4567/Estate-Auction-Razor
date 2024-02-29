using Repository.Interfaces.Auction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services.Auction
{
	public class AuctionReceiptPaymentRepositoryServices
	{
		private readonly IAuctionReceiptPaymentRepository _auctionReceiptPaymentRepository;

		public AuctionReceiptPaymentRepositoryServices(IAuctionReceiptPaymentRepository auctionReceiptPaymentRepository)
		{
			_auctionReceiptPaymentRepository = auctionReceiptPaymentRepository;
		}
	}
}
