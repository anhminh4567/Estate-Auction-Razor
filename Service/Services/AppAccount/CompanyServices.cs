﻿using Repository.Interfaces.AppAccount;
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
using Service.Services.RealEstate;
using Service.Services.Auction;
using Repository.Interfaces.DbTransaction;
using Service.Interfaces.AppAccount;

namespace Service.Services.AppAccount
{
	public class CompanyServices : ICompanyServices
	{
		private readonly IUnitOfWork _unitOfWork;
        private readonly EstateServices _estateServices;
        private readonly AuctionServices _auctionService;
        public CompanyServices(IUnitOfWork unitOfWork, EstateServices estateServices)
        {
            _unitOfWork = unitOfWork;
            _estateServices = estateServices;
        }


        //private readonly ICompanyRepository _companyRepository;
        //private readonly EstateServices _estateServices;
        //private readonly AuctionServices _auctionService;
        //public CompanyServices(
        //	ICompanyRepository companyRepository, 
        //	EstateServices estateServices,
        //	AuctionServices auctionService)
        //{
        //	_companyRepository = companyRepository;
        //	_estateServices = estateServices;
        //	_auctionService = auctionService;
        //}
        public async Task<Company> GetById(int id)
		{
			return await _unitOfWork.Repositories.companyRepository.GetAsync(id);
		}
		public async Task<List<Company>> GetAll() 
		{
			return await _unitOfWork.Repositories.companyRepository.GetAllAsync();
		}
		public async Task<List<Estate>> GetAllEstateByCompanyId(int companyId) 
		{
			return await _estateServices.GetByCompanyId(companyId);
		}
		public async Task<List<Repository.Database.Model.AuctionRelated.Auction>?> GetAllAuctionsByCompanyId(int companyId)
		{
			return await _auctionService.GetByCompanyId(companyId);
		}
		public async Task<Company> Create(Company company) 
		{
			return await _unitOfWork.Repositories.companyRepository.CreateAsync(company);
		}
		public async Task<bool> Update(Company company)
		{
			return await _unitOfWork.Repositories.companyRepository.UpdateAsync(company);
		}
		public async Task<bool> Delete(Company company)
		{
			return await _unitOfWork.Repositories.companyRepository.DeleteAsync(company);
		}
	}
}
