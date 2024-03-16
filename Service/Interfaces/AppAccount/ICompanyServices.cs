using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repository.Database.Model.AppAccount;
using Repository.Database.Model.RealEstate;

namespace Service.Interfaces.AppAccount
{
    public interface ICompanyServices
    {
        Task<Company> GetById(int id);
        Task<List<Company>> GetAll();
        Task<List<Estate>> GetAllEstateByCompanyId(int companyId);
        Task<List<Repository.Database.Model.AuctionRelated.Auction>?> GetAllAuctionsByCompanyId(int companyId);
        Task<Company> Create(Company company);
        Task<bool> Update(Company company);
        Task<bool> Delete(Company company);
    }
}
