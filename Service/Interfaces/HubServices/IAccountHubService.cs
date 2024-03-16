using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repository.Database.Model.AppAccount;

namespace Service.Interfaces.HubServices
{
    public interface IAccountHubService
    {
        Task SendNewAccount(Account account);
        Task SendUpdateAccount(Account account);
    }
}
