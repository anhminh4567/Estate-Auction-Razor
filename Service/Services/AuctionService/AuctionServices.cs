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
		private readonly EstateCategoriesServices _estateCategoriesServices;
		public AuctionServices(IAuctionRepository auctionRepository, EstateServices estateService, EstateCategoriesServices estateCategoriesServices)
		{
			_auctionRepository = auctionRepository;
			_estateServices = estateService;
			_estateCategoriesServices = estateCategoriesServices;
		}
		public async Task<Auction> GetById(int id)
		{
			return await _auctionRepository.GetAsync(id);
		}
		public async Task<List<Auction>> GetByCompanyId(int companyId)
		{
			var getEstates = (await _estateServices.GetByCompanyId(companyId)).Select(e => e.EstateId).ToArray();
			if (getEstates is null)
				throw new Exception("something wrong with get estate, it should never be null, only empty list or so");
			var result = new List<Auction>();	
			foreach(var estate in getEstates) 
			{
				var tryGetAuctions = await _auctionRepository.GetByEstateId(estate);
				if (tryGetAuctions is not null)
					result = result.Concat(tryGetAuctions).ToList();
			}
			return result;
		}
		public async Task<List<Auction>> GetAll()
		{
			return await _auctionRepository.GetAllAsync();
		}
		public async Task<List<Auction>> GetRange(int start, int amount) 
		{
			if (start < 0 || amount <= 0)
				throw new ArgumentException("start, amount must > 0");
			return await _auctionRepository.GetRange(start,amount);
		}
		public async Task<List<Auction>> GetRangeInclude_Estate_Company(int start, int amount) 
		{
            if (start < 0 || amount <= 0)
                throw new ArgumentException("start, amount must > 0");
			var result = await _auctionRepository.GetRange_IncludeEstate_Company(start,amount);
			foreach(var auction in result) 
			{
				var estateId = auction.EstateId;
				auction.Estate.EstateCategory = await _estateCategoriesServices.GetEstateCategoriesByEstateId(estateId);
			}
			return result;
		}
		public async Task<List<Auction>> GetByEstateId(int estateId)
		{
			return await _auctionRepository.GetByEstateId(estateId);
		}
		public async Task<Auction?> Create(Auction auction) 
		{
			var getEstate = await _estateServices.GetById(auction.EstateId);
			if(getEstate is not null) 
			{
				var tryGetEstateAuctionStatus = await _auctionRepository.GetByEstateId(getEstate.EstateId);
				foreach (var auc in tryGetEstateAuctionStatus) 
				{
					if(auction.Status != Repository.Database.Model.Enum.AuctionStatus.CANCELLED) // nghia la auction cho mieng dat nay chua bi huy, neu v thi ko duoc tao auction moi cho no
						throw new Exception("error in creeate auction, this is because the estate you select has already been in auction");
				}
			}
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

