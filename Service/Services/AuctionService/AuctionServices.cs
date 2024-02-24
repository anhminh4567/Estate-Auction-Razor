using Repository.Database.Model.AuctionRelated;
using Repository.Interfaces.Auction;
using Repository.Interfaces.RealEstate;
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
		private readonly IEstateRepository _estateRepository;
		public AuctionServices(IAuctionRepository auctionRepository, IEstateRepository estateRepository)
		{
			_auctionRepository = auctionRepository;
			_estateRepository = estateRepository;
		}
		public async Task<Auction> GetAuctionById(int id) 
		{
			return await _auctionRepository.GetAsync(id);
		}
		public async Task<List<Auction>> GetAuctionsByCompanyId(int id) 
		{
			var getEstate = await _estateRepository.GetByCompanyId(id);
			var getAuctions = await _auctionRepository.GetByEstateId(getEstate.EstateId);
			return getAuctions;
		}
		public async Task<List<Auction>> GetAllAuction() 
		{
			return await _auctionRepository.GetAllAsync();
		}
	}
}
