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
	public class TransactionService
	{
		private readonly ITransactionRepository _transactionRepository;
		public TransactionService(ITransactionRepository transactionRepository)
		{
			_transactionRepository = transactionRepository;
		}
		public async Task<Transaction?> GetTransaction(int id, bool includeAccountDetails = false) 
		{
			if(includeAccountDetails is false) 
				return await _transactionRepository.GetAsync(id);
			return await _transactionRepository.GetFullAsync(id); 
		}
		public async Task<Transaction?> CreateTransaction(Transaction transaction) 
		{
			return await _transactionRepository.CreateAsync(transaction);
		}
		public async Task<List<Transaction>> GetAllTransaction() 
		{
			return await _transactionRepository.GetAllAsync();
		}
		public async Task<bool> UpdateTransaction(Transaction transaction) 
		{
			return await _transactionRepository.UpdateAsync(transaction);
		}
		public async Task<bool> DeleteTransaction(Transaction transaction) 
		{
			return await _transactionRepository.DeleteAsync(transaction);
		}
		public async Task<bool> DeleteTransaction(int id) 
		{
			var getTransaction = await GetTransaction(id);
			if (getTransaction is null) 
				return false;
			return await DeleteTransaction(getTransaction);
		}
	}
}
