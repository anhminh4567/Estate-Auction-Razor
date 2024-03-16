using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repository.Database.Model.RealEstate;

namespace Service.Interfaces.RealEstate
{
    public interface IEstateCategoryDetailServices
    {
        Task<EstateCategoryDetail> GetById(int id);
        Task<List<EstateCategoryDetail>> GetAll();
        Task<List<EstateCategoryDetail>> GetRange(params int[] categoryDetailIds);
        Task<EstateCategoryDetail> Create(EstateCategoryDetail estateCategoryDetail);
        Task<bool> UpdateCategoryDetail(EstateCategoryDetail estateCategoryDetail);
        Task<bool> DeleteCategoryDetail(EstateCategoryDetail estateCategoryDetail);
        Task<bool> CheckForDuplicateName(EstateCategoryDetail estateCategoryDetail);
    }
}
