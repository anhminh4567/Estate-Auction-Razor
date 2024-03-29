﻿using Repository.Database.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Interfaces
{
	public interface ITransactionRepository : ICrud<Transaction>
	{
		Task<Transaction?> GetFullAsync(int id);
		Task<List<Transaction>> GetByAccountId(int accountId);
	}
}
