using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Repository.Database.Model.AuctionRelated;

namespace Service.Interfaces.Auction
{
    public interface IAuctionReceiptServices
    {
        Task<AuctionReceipt?> GetById(int id, string includeProperties = null);
        Task<AuctionReceipt?> GetByAuctionId(int auctionId, string includeProperties = null);
        Task<AuctionReceipt?> GetIncludes(int id, string includeProperties);
        Task<List<AuctionReceipt>> GetWithCondition(Expression<Func<AuctionReceipt, bool>> expression = null,
                    Func<IQueryable<AuctionReceipt>, IOrderedQueryable<AuctionReceipt>> orderBy = null,
                    string includeProperties = "");
        Task<List<AuctionReceipt>> GetAll();
        Task<AuctionReceipt> Create(AuctionReceipt auctionReceipt);
        Task<bool> Update(AuctionReceipt auctionReceipt);
        Task<bool> Delete(AuctionReceipt auctionReceipt);
    }
}
