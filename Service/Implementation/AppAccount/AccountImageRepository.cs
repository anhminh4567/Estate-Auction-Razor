using Repository.Database.Model.AppAccount;
using Repository.Database;
using Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Service.Implementation.AppAccount
{
    public class AccountImageRepository : IAccountImageRepository
    {
        private readonly AuctionRealEstateDbContext _context;
        public AccountImageRepository(AuctionRealEstateDbContext context)
        {
            _context = context;
        }

        public async Task<List<AccountImages>> GetAllAsync()
        {
            return await _context.AccountImages.ToListAsync();
        }

        public async Task<AccountImages> GetAsync(int id)
        {
            return await _context.AccountImages.FirstOrDefaultAsync(c => c.AccountId == id);
        }
        public async Task<AccountImages> CreateAsync(AccountImages t)
        {
            var newCompany = await _context.AccountImages.AddAsync(t);
            await _context.SaveChangesAsync();
            return newCompany.Entity;
        }

        public async Task<bool> DeleteAsync(AccountImages t)
        {
            if (t == null)
            {
                return false;
            }
            _context.AccountImages.Remove(t);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<bool> UpdateAsync(AccountImages t)
        {
            if (t == null)
            {
                return false;
            }
            _context.AccountImages.Update(t);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
