﻿using Repository.Database.Model;
using Repository.Database.Model.AppAccount;
using Repository.Database.Model.AuctionRelated;
using Repository.Database.Model.Enum;
using Repository.Interfaces;
using Repository.Interfaces.AppAccount;
using Repository.Interfaces.DbTransaction;
using Service.Interfaces.AppAccount;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Service.Services.AppAccount
{
	public class AccountServices 
	{
		private readonly IUnitOfWork _unitOfWork;

        public AccountServices(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        //private readonly IAccountRepository _accountRepository;
        //public AccountServices(IAccountRepository accountRepository) 
        //{
        //	_accountRepository = accountRepository;
        //}
		private async Task<List<Account>> GetWithCondition(Expression<Func<Account, bool>> expression = null,
            Func<IQueryable<Account>, IOrderedQueryable<Account>> orderBy = null,
            string includeProperties = "")
		{
			return await _unitOfWork.Repositories.accountRepository.GetByCondition(expression,orderBy,includeProperties);
		}
		public async Task<List<Account>> GetAllCustomers()
		{
			return await GetWithCondition(a => a.Role == Repository.Database.Model.Enum.Role.CUSTOMER);
		}
        public async Task<List<Account>> GetAllCompany()
        {
            return await GetWithCondition(a => a.Role == Repository.Database.Model.Enum.Role.COMPANY);
        }
        public async Task<List<Account>> GetActiveCustomers()
		{
			return await _unitOfWork.Repositories.accountRepository.GetActiveCustomers();	
		}
		public async Task< Account> GetById(int id) 
		{
			return await _unitOfWork.Repositories.accountRepository.GetAsync( id );	
		}
		public async Task<Account> GetByEmail(string email, bool isCaseSensitive = false) 
		{
			return await _unitOfWork.Repositories.accountRepository.GetByEmail( email ,isCaseSensitive);
		}
		public async Task<Account> GetByEmailPassword(string email, string password) 
		{
			return await _unitOfWork.Repositories.accountRepository.GetByEmailPassword(email, password);
		}
		public async Task<Account?> GetInclude(int accountId, string includeProperties = "")
		{
			if (accountId == 0) 
				throw new ArgumentNullException(nameof(accountId));
			return (await _unitOfWork.Repositories.accountRepository.GetByCondition(a => a.AccountId == accountId, includeProperties: includeProperties)).FirstOrDefault();
		}
		public async Task<bool> IsEmailExisted(string email) 
		{
			var getAcc = await GetByEmail(email);
			return getAcc is not null ? true // not null mean existed
				 : false;
		}
		public async Task<bool> IsAccountActive(Account account)
		{
			if (account.Status.Equals(AccountStatus.ACTIVED))
				return true;
			return false;
		}
		public async Task<Account?> Create(Account newAccount) 
		{
			try
			{
                if (await IsEmailExisted(newAccount.Email))
                {
                    return null;
                }
                var result= await _unitOfWork.Repositories.accountRepository.CreateAsync(newAccount);
				if (result != null)
				{
					return result;
				}
				else
					throw new Exception("error create");
			}
            catch (Exception ex) 
			{
				return null;
			}
            
		}
		public async Task<bool> Update(Account account)
		{
            try
            {
                var result = await _unitOfWork.Repositories.accountRepository.UpdateAsync(account);
                if (result)
                {
                    return result;
                }
                else
                    throw new Exception("error create");
            }
            catch (Exception ex)
            {
                return false;
            }
		}
		public async Task<bool> Delete(Account account) 
		{
            try
            {
                var result = await _unitOfWork.Repositories.accountRepository.DeleteAsync(account);
                if (result)
                {
                    return result;
                }
                else
                    throw new Exception("error create");
            }
            catch (Exception ex)
            {
                return false;
            }
		}
		public async Task<(bool IsSuccess, string? message)> AdminBanUser(Account account)
		{
			if (account == null)
				return (false, "cannot found account");
			account.Status = AccountStatus.DEACTIVED;
			var updateResult = await _unitOfWork.Repositories.accountRepository.UpdateAsync(account);
			if (updateResult)
				return (true, "Success");
			return (false, "fail to update");
		}
        public async Task<(bool IsSuccess, string? message)> AdminActiveUser(Account account)
        {
            if (account == null)
                return (false, "cannot found account");
            account.Status = AccountStatus.ACTIVED;
            var updateResult = await _unitOfWork.Repositories.accountRepository.UpdateAsync(account);
            if (updateResult)
                return (true, "Success");
            return (false, "fail to update");
        }
    }
}
