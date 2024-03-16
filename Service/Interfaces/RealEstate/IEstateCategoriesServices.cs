using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repository.Database.Model.RealEstate;

namespace Service.Interfaces.RealEstate
{
    public interface IEstateCategoriesServices
    {
        Task<List<EstateCategories>> GetEstateCategoriesByEstateId(int estateId);
        Task<EstateCategories?> GetEstateCategory(int categoryId, int estateId);
        Task<List<EstateCategories>> GetEstateCategoriesByCategoryId(int categoryId);
        Task<EstateCategories> CreateEstateCategories(EstateCategories estateCategories);
        Task<List<EstateCategories>> CreateRangeAsync(List<EstateCategories> estateCategories);
        Task<bool> UpdateEstateCategories(EstateCategories estateCategories);
        Task<bool> DeleteEstateCategories(EstateCategories estateCategories);
        Task<bool> DeleteRangeAsync(List<EstateCategories> estateCategories);
        Task<bool> CheckForCategoryDetailInUsed(int categoryId);
    }
}
