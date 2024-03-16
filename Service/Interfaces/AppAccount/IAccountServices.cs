using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Repository.Database.Model.AppAccount;

namespace Service.Interfaces.AppAccount
{
    public interface IAccountServices
    {
        Task<List<Account>> GetWithCondition(Expression<Func<Account, bool>> expression = null,
            Func<IQueryable<Account>, IOrderedQueryable<Account>> orderBy = null,
            string includeProperties = "");
        Task<List<Account>> GetAllCustomers();
        Task<List<Account>> GetAllCompany();
        Task<List<Account>> GetActiveCustomers();
        Task<Account> GetById(int id);
        Task<Account> GetByEmail(string email, bool isCaseSensitive = false);
        Task<Account> GetByEmailPassword(string email, string password);
        Task<Account?> GetInclude(int accountId, string includeProperties = "");
        Task<bool> IsEmailExisted(string email);
        Task<bool> IsAccountActive(Account account);
        Task<Account?> Create(Account newAccount);
        Task<bool> Update(Account account);
        Task<bool> Delete(Account account);
        Task<(bool IsSuccess, string? message)> AdminBanUser(Account account);
        Task<(bool IsSuccess, string? message)> AdminActiveUser(Account account);
    }
}
