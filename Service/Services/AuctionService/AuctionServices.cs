using Repository.Database.Model.AuctionRelated;
using Repository.Interfaces.Auction;
using Repository.Interfaces.RealEstate;
using Service.Services.RealEstate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services.AuctionService
{
	public class AuctionServices
	{
		private readonly IAuctionRepository _auctionRepository;
		private readonly EstateServices _estateServices;
		public AuctionServices(IAuctionRepository auctionRepository, EstateServices estateService)
		{
			_auctionRepository = auctionRepository;
			_estateServices = estateService;
		}
		public async Task<Auction> GetById(int id)
		{
			return await _auctionRepository.GetAsync(id);
		}
		public async Task<List<Auction>?> GetByCompanyId(int companyId)
		{
			var getEstates = (await _estateServices.GetByCompanyId(companyId)).Select(e => e.EstateId).ToArray();
			if (getEstates is null || getEstates.Any() == false)
				return null;
			var result = new List<Auction>();	
			foreach(var estate in getEstates) 
			{
				var tryGetAuctions = await _auctionRepository.GetByEstateId(estate);
				if (tryGetAuctions is not null)
					result.Concat(tryGetAuctions);
			}
			return (result.Count == 0) 
				? null 
				: result;
		}
		public async Task<List<Auction>> GetAll()
		{
			return await _auctionRepository.GetAllAsync();
		}
		public async Task<List<Auction>> GetByEstateId(int estateId)
		{
			return await _auctionRepository.GetByEstateId(estateId);
		}
		public async Task<Auction> Create(Auction auction) 
		{
			return await _auctionRepository.CreateAsync(auction);
		}
		public async Task<bool> Delete(Auction auction)
		{
			return await _auctionRepository.DeleteAsync(auction);
		}
		public async Task<bool> Update(Auction auction)
		{
			return await _auctionRepository.UpdateAsync(auction);
		}
	}
}

