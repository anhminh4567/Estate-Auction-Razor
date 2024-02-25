using Repository.Database.Model.RealEstate;
using Repository.Interfaces.RealEstate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services.RealEstate
{
	public class EstateServices
	{
		private readonly IEstateRepository _estateRepository;

		public EstateServices(IEstateRepository estateRepository)
		{
			_estateRepository = estateRepository;
		}
		public async Task<Estate> GetById(int id) 
		{
			return await _estateRepository.GetAsync(id);
		}
		public async Task<List<Estate>> GetByCompanyId(int companyId) 
		{
			return await _estateRepository.GetByCompanyId(companyId);
		}
		public async Task<List<Estate>> GetAll()
		{
			return await _estateRepository.GetAllAsync();
		}
		public async Task<Estate> Create(Estate estate)
		{
			return await _estateRepository.CreateAsync(estate);
		}
		public async Task<bool> Update(Estate estate)
		{
			return await _estateRepository.UpdateAsync(estate);
		}
		public async Task<bool> Delete(Estate estate)
		{
			return await _estateRepository.DeleteAsync(estate);
		}
	}
}
