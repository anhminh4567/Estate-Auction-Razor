using Microsoft.AspNetCore.SignalR;
using Repository.Database.Model.AppAccount;
using Repository.Interfaces.DbTransaction;

namespace Service.MyHub.HubServices
{
	public class AccountHubService
	{
		private readonly IHubContext<AccountHub> _accountHub;
		private readonly IUnitOfWork _unitOfWork;

		public AccountHubService(IHubContext<AccountHub> accountHub, IUnitOfWork unitOfWork)
		{
			_accountHub = accountHub;
			_unitOfWork = unitOfWork;
		}
		public async Task SendNewAccount(Account account)
		{
			await _accountHub.Clients.All.SendAsync("OnNewAccount", account);
		}
		public async Task SendUpdateAccount(Account account)
		{
			await _accountHub.Clients.All.SendAsync("OnUpdateAccount", account);
		}
	}
}
