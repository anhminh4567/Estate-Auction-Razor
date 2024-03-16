using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Interfaces
{
    public interface IDatabaseTransactionService
    {
        Task BeginTransaction();
        Task CommitTransaction();
        Task RollBack();
        Task SaveChanges();
    }
}
