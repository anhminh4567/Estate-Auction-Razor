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
		public async Task CreateVnpayTransaction() 
		{
			
		}
	}
}
