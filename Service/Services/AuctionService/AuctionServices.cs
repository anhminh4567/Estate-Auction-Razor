using Repository.Database.Model.AuctionRelated;
using Repository.Interfaces.Auction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services.AuctionService
{
	public class AuctionServices
	{
		private readonly IAuctionRepository _auctionRepository;
		public AuctionServices(IAuctionRepository auctionRepository)
		{
			_auctionRepository = auctionRepository;
		}
		public async Task<List<Auction>> GetAllAuction() 
		{
			return await _auctionRepository.GetAllAsync();
		}
	}
}
