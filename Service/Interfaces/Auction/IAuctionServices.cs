using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Service.Interfaces.Auction
{
    public interface IAuctionServices
    {
        Task<bool> CheckForJoinedAuction(int accountId, int auctionId);
        Task<List<Repository.Database.Model.AuctionRelated.Auction>> GetActiveAuctions();
        Task<Repository.Database.Model.AuctionRelated.Auction> GetById(int id);
        Task<List<Repository.Database.Model.AuctionRelated.Auction>> GetByCompanyId(int companyId);
        Task<List<Repository.Database.Model.AuctionRelated.Auction>> GetAll();
        Task<List<Repository.Database.Model.AuctionRelated.Auction>> GetRange(int start, int amount);
        Task<Repository.Database.Model.AuctionRelated.Auction?> GetInclude(int auctionId, string includes);
        Task<List<Repository.Database.Model.AuctionRelated.Auction>> GetRangeInclude_Estate_Company(int start, int amount);
        Task<List<Repository.Database.Model.AuctionRelated.Auction>> GetAuctionsInclude(Expression<Func<Repository.Database.Model.AuctionRelated.Auction, bool>> expression = null,
                    Func<IQueryable<Repository.Database.Model.AuctionRelated.Auction>, IOrderedQueryable<Repository.Database.Model.AuctionRelated.Auction>> orderBy = null,
                    string includeProperties = "");
        Task<List<Repository.Database.Model.AuctionRelated.Auction>> GetByEstateId(int estateId);
        Task<List<Repository.Database.Model.AuctionRelated.Auction>> GetWithCondition(
            Expression<Func<Repository.Database.Model.AuctionRelated.Auction, bool>> expression = null,
            Func<IQueryable<Repository.Database.Model.AuctionRelated.Auction>, IOrderedQueryable<Repository.Database.Model.AuctionRelated.Auction>> orderBy = null,
            string includeProperties = "");
        Task<(bool IsSuccess, string? message, Repository.Database.Model.AuctionRelated.Auction?)> Create(Repository.Database.Model.AuctionRelated.Auction auction);
        Task<(bool IsSuccess, string? message)> CancelAuction(Repository.Database.Model.AuctionRelated.Auction auction);
        Task<(bool IsSuccess, string? message)> AdminForceCancelAuction(Repository.Database.Model.AuctionRelated.Auction auction);
        Task<bool> Update(Repository.Database.Model.AuctionRelated.Auction auction);


    }
}
