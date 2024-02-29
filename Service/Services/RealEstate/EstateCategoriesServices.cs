﻿using Repository.Database.Model.RealEstate;
using Repository.Interfaces.RealEstate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services.RealEstate
{
	public class EstateCategoriesServices
	{
		private readonly IEstateCategoriesRepository _estateCategoriesRepository;

		public EstateCategoriesServices(IEstateCategoriesRepository estateCategoriesRepository)
		{
			_estateCategoriesRepository = estateCategoriesRepository;
		}
		public async Task<List<EstateCategories>> GetEstateCategoriesByEstateId(int estateId)
		{
			return await _estateCategoriesRepository.GetByEstateId(estateId);
		}
        public async Task<EstateCategories?> GetEstateCategory(int categoryId, int estateId)
        {
			return (await _estateCategoriesRepository.GetByCondition(c =>
				c.CategoryId == categoryId &&
				c.EstateId == estateId)).FirstOrDefault();
        }
        public async Task<List<EstateCategories>> GetEstateCategoriesByCategoryId(int categoryId)
		{
			return await _estateCategoriesRepository.GetByCategoryId(categoryId);
		}
		public async Task<EstateCategories> CreateEstateCategories(EstateCategories estateCategories) 
		{
			return await _estateCategoriesRepository.CreateAsync(estateCategories);
		}
		public async Task<List<EstateCategories>> CreateRangeAsync(List<EstateCategories> estateCategories) 
		{
			var result = new List<EstateCategories>();
			foreach(var estateCategory in estateCategories) 
			{
				var res =await  _estateCategoriesRepository.CreateAsync(estateCategory);
				if(res is not null)
					result.Add(res);
			}
			return result;
		}
		public async Task<bool> UpdateEstateCategories(EstateCategories estateCategories) 
		{
			return await _estateCategoriesRepository.UpdateAsync(estateCategories);
		}
		public async Task<bool> DeleteEstateCategories(EstateCategories estateCategories) 
		{
			return await _estateCategoriesRepository.DeleteAsync(estateCategories);
		}
		public async Task<bool> DeleteRangeAsync(List<EstateCategories> estateCategories) 
		{
			return await _estateCategoriesRepository.DeleteRange(estateCategories);
		}
		public async Task<bool> CheckForCategoryDetailInUsed(int categoryId)
		{
			var list = await _estateCategoriesRepository.GetByCategoryId(categoryId);
			return list.Any();
		}
	}
}