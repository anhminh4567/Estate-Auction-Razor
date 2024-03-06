using Repository.Interfaces.DbTransaction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services
{
    public class DatabaseTransactionService
    {
        private readonly IUnitOfWork _unitOfWork;
        public DatabaseTransactionService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task BeginTransaction()
        {
            await _unitOfWork.BeginTransaction();
        }
        public async Task CommitTransaction()
        {
            await _unitOfWork.CommitAsync();
        }
        public async Task RollBack()
        {
            await _unitOfWork.RollBackAsync();
        }
        public async Task SaveChanges() 
        { 
            await _unitOfWork.SaveChangesAsync(); 
        }
    }
}
