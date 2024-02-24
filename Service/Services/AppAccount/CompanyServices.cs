using Repository.Interfaces.AppAccount;
using Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repository.Database.Model.AppAccount;

namespace Service.Services.AppAccount
{
	public class CompanyServices
	{
		private readonly IAccountImageRepository _accountImageRepository;
		private readonly ICompanyRepository _companyRepository;
		private readonly IImagesRepository _imageRepository;
		public CompanyServices(ICompanyRepository companyRepository, IAccountImageRepository accountImageRepository, IImagesRepository imagesRepository)
		{
			_accountImageRepository = accountImageRepository;
			_companyRepository = companyRepository;
			_imageRepository = imagesRepository;
		}
		public async Task<Company> GetCompanyById(int id)
		{
			return await _companyRepository.GetAsync(id);
		}
	}
}
