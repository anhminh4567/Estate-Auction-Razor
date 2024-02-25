using Repository.Interfaces.AppAccount;
using Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repository.Database.Model.AppAccount;
using Repository.Database.Model.RealEstate;
using Repository.Interfaces.RealEstate;
using Repository.Database.Model.AuctionRelated;
using Repository.Interfaces.Auction;
using Service.Services.AuctionService;
using Service.Services.RealEstate;

namespace Service.Services.AppAccount
{
	public class CompanyServices
	{
		private readonly ICompanyRepository _companyRepository;
		private readonly EstateServices _estateServices;
		private readonly EstateImagesServices _estateImagesServices;
		private readonly AuctionServices _auctionService;
		public CompanyServices(
			ICompanyRepository companyRepository, 
			EstateServices estateServices,
			AuctionServices auctionService,
			EstateImagesServices estateImagesServices)
		{
			_companyRepository = companyRepository;
			_estateServices = estateServices;
			_auctionService = auctionService;
			_estateImagesServices = estateImagesServices;
		}
		public async Task<Company> GetById(int id)
		{
			return await _companyRepository.GetAsync(id);
		}
		public async Task<List<Company>> GetAll() 
		{
			return await _companyRepository.GetAllAsync();
		}
		public async Task<List<Estate>> GetAllEstateByCompanyId(int companyId) 
		{
			return await _estateServices.GetByCompanyId(companyId);
		}
		public async Task<List<Auction>> GetAllAuctionsByCompanyId(int companyId) 
		{
			var getAllEstateId = ( await GetAllEstateByCompanyId(companyId)).Select(e => e.EstateId);
			var result = new List<Auction>();
			foreach (var id in getAllEstateId) 
			{
				result.Concat(await _auctionService.GetAuctionsByEstateId(id));
			}
			return result;
		}
	}
}
