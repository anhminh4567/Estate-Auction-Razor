﻿using Microsoft.EntityFrameworkCore;
using Repository.Database;
using Repository.Database.Model.AppAccount;
using Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Implementation.AppAccount
{
    public class AccountRepository : IAccountRepository
    {
        private readonly AuctionRealEstateDbContext _context;
        public AccountRepository(AuctionRealEstateDbContext context)
        {
            _context = context;
        }

        public async Task<List<Account>> GetAllAsync()
        {
            return await _context.Accounts.ToListAsync();
        }

        public async Task<Account> GetAsync(int id)
        {
            return await _context.Accounts.FirstOrDefaultAsync(c => c.AccountId == id);
        }
        public async Task<Account> CreateAsync(Account t)
        {
            var newAccount = await _context.Accounts.AddAsync(t);
            await _context.SaveChangesAsync();
            return newAccount.Entity;
        }

        public async Task<bool> DeleteAsync(Account t)
        {
            if (t == null)
            {
                return false;
            }
            _context.Accounts.Remove(t);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<bool> UpdateAsync(Account t)
        {
            if (t == null)
            {
                return false;
            }
            _context.Accounts.Update(t);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Account> GetByEmailPassword(string email, string password)
        {
            var getAccount = await _context.Accounts.FirstOrDefaultAsync(a => a.Email == email && a.Password == password);
            return getAccount;
        }
    }
}