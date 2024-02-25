using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Pqc.Crypto.Falcon;
using Repository.Database.Model;
using Repository.Interfaces;
using Service.Services.VnpayService.VnpayUtility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services
{
	public class TransactionServices
	{
		private readonly ITransactionRepository _transactionRepository;
		public TransactionServices(ITransactionRepository transactionRepository)
		{
			_transactionRepository = transactionRepository;
		}
		public async Task<Transaction?> Get(int id, bool includeAccountDetails = false) 
		{
			if(includeAccountDetails is false) 
				return await _transactionRepository.GetAsync(id);
			return await _transactionRepository.GetFullAsync(id); 
		}
		public  async Task<List<Transaction>?> GetByAccountId(int accountId) 
		{
			return await _transactionRepository.GetByAccountId(accountId);
		}
		public async Task<Transaction?> Create(Transaction transaction) 
		{
			return await _transactionRepository.CreateAsync(transaction);
		}
		public async Task<List<Transaction>> GetAllTransaction() 
		{
			return await _transactionRepository.GetAllAsync();
		}
		public async Task<bool> Update(Transaction transaction) 
		{
			return await _transactionRepository.UpdateAsync(transaction);
		}
		public async Task<bool> Delete(Transaction transaction) 
		{
			return await _transactionRepository.DeleteAsync(transaction);
		}
		public async Task<bool> Delete(int id) 
		{
			var getTransaction = await Get(id);
			if (getTransaction is null) 
				return false;
			return await Delete(getTransaction);
		}
	}
}
