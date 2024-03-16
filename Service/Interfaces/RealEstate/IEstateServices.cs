using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repository.Database.Model.RealEstate;

namespace Service.Interfaces.RealEstate
{
    public interface IEstateServices
    {
        Task<Estate?> GetById(int id);
        Task<Estate?> GetFullDetail(int id);
        Task<Estate?> GetIncludes(int id, params string[] attributeName);
        Task<List<Estate>> GetAllDetails();
        Task<Estate?> GetWithCondition(int id, string properties);
        Task<List<Estate>> GetByCompanyId(int companyId);
        Task<List<Estate>> GetAll();
        Task<(bool IsSuccess, string? message, Estate? NewEstate)> Create(Estate estate, List<string> SelectedEstateCategoriesOptions);
        Task<(bool IsSuccess, string message)> UpdateEstates(Estate estate, List<int> SelectedCategories);
        Task<(bool IsSuccess, string message)> DeleteEstate(Estate estate);
        Task<(bool IsSuccess, string? message)> AdminBannedEstate(Estate estate);
    }
}
