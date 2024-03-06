using Repository.Database.Model.RealEstate;
using Repository.Interfaces.DbTransaction;
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
		private readonly IUnitOfWork _unitOfWork;

        public EstateCategoryDetailServices(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        //private readonly IEstateCategoryDetailRepository _estateCategoryDetailRepository;

        //public EstateCategoryDetailServices(IEstateCategoryDetailRepository estateCategoryDetailRepository)
        //{
        //	_estateCategoryDetailRepository = estateCategoryDetailRepository;
        //}
        public async Task<EstateCategoryDetail> GetById(int id) 
		{
			return await _unitOfWork.Repositories.estateCategoryDetailRepository.GetAsync(id);
		}
		public async Task<List<EstateCategoryDetail>> GetAll() 
		{
			return await _unitOfWork.Repositories.estateCategoryDetailRepository.GetAllAsync();
		}
		public async Task<List<EstateCategoryDetail>> GetRange(params int[] categoryDetailIds) 
		{
			return await _unitOfWork.Repositories.estateCategoryDetailRepository.GetRange(categoryDetailIds);
		}
		public async Task<EstateCategoryDetail> Create(EstateCategoryDetail estateCategoryDetail) 
		{
			return await _unitOfWork.Repositories.estateCategoryDetailRepository.CreateAsync(estateCategoryDetail);
		}
		public async Task<bool> UpdateCategoryDetail(EstateCategoryDetail estateCategoryDetail) 
		{
			var item = await _unitOfWork.Repositories.estateCategoryDetailRepository.GetAsync(estateCategoryDetail.CategoryId);
			item.CategoryName = estateCategoryDetail.CategoryName;
			item.Description = estateCategoryDetail.Description;
			return await _unitOfWork.Repositories.estateCategoryDetailRepository.UpdateAsync(item);
		}
		public async Task<bool> DeleteCategoryDetail(EstateCategoryDetail estateCategoryDetail) 
		{
			return await _unitOfWork.Repositories.estateCategoryDetailRepository.DeleteAsync(estateCategoryDetail);
		}
		public async Task<bool> CheckForDuplicateName(EstateCategoryDetail estateCategoryDetail)
		{
			var list = await _unitOfWork.Repositories.estateCategoryDetailRepository.GetAllAsync();
			return list.Where(p => p.CategoryId != estateCategoryDetail.CategoryId && p.CategoryName.ToLower().Equals(estateCategoryDetail.CategoryName.ToLower())).Any();
		}
	}
}
