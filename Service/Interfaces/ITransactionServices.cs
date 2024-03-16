using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repository.Database.Model;

namespace Service.Interfaces
{
    public interface ITransactionServices
    {
        Task<Transaction?> GetById(int id);
        Task<Transaction?> GetInclude(int id, string includeProperties);
        Task<List<Transaction>> GetByAccountId(int accountId);
        Task<Transaction?> Create(Transaction transaction);
        Task<List<Transaction>> GetAllTransaction();
        Task<bool> Update(Transaction transaction);
        Task<bool> Delete(Transaction transaction);
        Task<bool> Delete(int id);
        Task<(bool IsSuccess, string? message)> AdminCreateTransaction(Transaction transaction);
    }
}
