using Repository.Database.Model;
using Repository.Database.Model.AppAccount;
using Repository.Database.Model.AuctionRelated;
using Repository.Interfaces;
using Repository.Interfaces.AppAccount;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services.AppAccount
{
	public class AccountServices
	{
		private readonly IAccountRepository _accountRepository;
		public AccountServices(IAccountRepository accountRepository) 
		{
			_accountRepository = accountRepository;
		}
		public async Task< Account> GetById(int id) 
		{
			return await _accountRepository.GetAsync( id );	
		}
		public async Task<Account> GetByEmail(string email, bool isCaseSensitive = false) 
		{
			return await _accountRepository.GetByEmail( email ,isCaseSensitive);
		}
		public async Task<Account> GetByEmailPassword(string email, string password) 
		{
			return await _accountRepository.GetByEmailPassword(email, password);
		}
		public async Task<Account?> GetInclude(int accountId, string includeProperties = "")
		{
			if (accountId == 0) 
				throw new ArgumentNullException(nameof(accountId));
			return (await _accountRepository.GetByCondition(a => a.AccountId == accountId, includeProperties: includeProperties)).FirstOrDefault();
		}
		public async Task<bool> IsEmailExisted(string email) 
		{
			var getAcc = await GetByEmail(email);
			return getAcc is not null ? true // not null mean existed
				 : false;
		}
		public async Task<Account?> Create(Account newAccount) 
		{
			return await _accountRepository.CreateAsync(newAccount);
		}
		public async Task<bool> Update(Account account)
		{
			return await _accountRepository.UpdateAsync(account);
		}
		public async Task<bool> Delete(Account account) 
		{
			return await _accountRepository.DeleteAsync(account);
		}
	}
}
