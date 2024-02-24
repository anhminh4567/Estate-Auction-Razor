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
		private readonly IAccountImageRepository _accountImageRepository;
		private readonly IAccountRepository _accountRepository;
		private readonly IImagesRepository _imageRepository;
		public AccountServices(IAccountRepository accountRepository, IAccountImageRepository accountImageRepository, IImagesRepository imagesRepository ) 
		{
			_accountImageRepository = accountImageRepository;
			_accountRepository = accountRepository;
			_imageRepository = imagesRepository;
		}
		public async Task< Account> GetAccountById(int id) 
		{
			return await _accountRepository.GetAsync( id );	
		}
		public async Task<Account> GetAccountByEmail(string email, bool isCaseSensitive = false) 
		{
			return await _accountRepository.GetByEmail( email ,isCaseSensitive);
		}
		public async Task<Account> GetAccountByEmailPassword(string email, string password) 
		{
			return await _accountRepository.GetByEmailPassword(email, password);
		}
		public async Task<bool> IsEmailExisted(string email) 
		{
			var getAcc = await GetAccountByEmail(email);
			return getAcc is not null ? true // not null mean existed
				 : false;
		}
		public async Task<List<AppImage>> GetAccountImagesPath(int accountId) 
		{ 
			var accountFullDetail = await _accountRepository.GetFullAsync(accountId);
			return accountFullDetail.AccountImages.Select(a => a.Image).ToList();
		}
		public async Task<List<Transaction>> GetAccountTransaction(int accountId)
		{
			var accountFullDetail = await _accountRepository.GetFullAsync(accountId);
			return accountFullDetail.Transactions.ToList();
		}
	}
}
