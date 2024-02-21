using Microsoft.EntityFrameworkCore;
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
    public class CompanyRepository : ICompanyRepository
    {
        private readonly AuctionRealEstateDbContext _context;
        public CompanyRepository(AuctionRealEstateDbContext context)
        {
            _context = context;
        }

        public async Task<List<Company>> GetAllAsync()
        {
            return await _context.Companys.ToListAsync();
        }

        public async Task<Company> GetAsync(int id)
        {
            return await _context.Companys.FirstOrDefaultAsync(c => c.AccountId == id);
        }
        public async Task<Company> CreateAsync(Company t)
        {
            var newCompany = await _context.Companys.AddAsync(t);
            await _context.SaveChangesAsync();
            return newCompany.Entity;
        }

        public async Task<bool> DeleteAsync(Company t)
        {
            if (t == null)
            {
                return false;
            }
            _context.Companys.Remove(t);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<bool> UpdateAsync(Company t)
        {
            if (t == null)
            {
                return false;
            }
            _context.Companys.Update(t);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
