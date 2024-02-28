using Repository.Database.Model.RealEstate;
using Repository.Interfaces.RealEstate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services.RealEstate
{
	public class EstateCategoryDetailServices
	{
		private readonly IEstateCategoryDetailRepository _estateCategoryDetailRepository;

		public EstateCategoryDetailServices(IEstateCategoryDetailRepository estateCategoryDetailRepository)
		{
			_estateCategoryDetailRepository = estateCategoryDetailRepository;
		}
		public async Task<EstateCategoryDetail> GetById(int id) 
		{
			return await _estateCategoryDetailRepository.GetAsync(id);
		}
		public async Task<List<EstateCategoryDetail>> GetAll() 
		{
			return await _estateCategoryDetailRepository.GetAllAsync();
		}
		public async Task<List<EstateCategoryDetail>> GetRange(params int[] categoryDetailIds) 
		{
			return await _estateCategoryDetailRepository.GetRange(categoryDetailIds);
		}
		public async Task<EstateCategoryDetail> Create(EstateCategoryDetail estateCategoryDetail) 
		{
			return await _estateCategoryDetailRepository.CreateAsync(estateCategoryDetail);
		}
		public async Task<bool> UpdateCategoryDetail(EstateCategoryDetail estateCategoryDetail) 
		{
			var item = await _estateCategoryDetailRepository.GetAsync(estateCategoryDetail.CategoryId);
			item.CategoryName = estateCategoryDetail.CategoryName;
			item.Description = estateCategoryDetail.Description;
			return await _estateCategoryDetailRepository.UpdateAsync(item);
		}
		public async Task<bool> DeleteCategoryDetail(EstateCategoryDetail estateCategoryDetail) 
		{
			return await _estateCategoryDetailRepository.DeleteAsync(estateCategoryDetail);
		}
		public async Task<bool> CheckForDuplicateName(EstateCategoryDetail estateCategoryDetail)
		{
			var list = await _estateCategoryDetailRepository.GetAllAsync();
			return list.Where(p => p.CategoryId != estateCategoryDetail.CategoryId && p.CategoryName.ToLower().Equals(estateCategoryDetail.CategoryName.ToLower())).Any();
		}
	}
}
