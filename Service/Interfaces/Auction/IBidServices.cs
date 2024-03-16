using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repository.Database.Model.AuctionRelated;

namespace Service.Interfaces.Auction
{
    public interface IBidServices
    {
        Task<List<Bid>> GetByAccountId(int accountId);
        Task<List<Bid>> GetByAuctionId(int auctionId);
        Task<Bid?> GetHighestBids(int auctionId);
        Task<List<Bid>> GetByAuctionId_AccountId(int auctionId, int accountId);
        Task<Bid> Create(Bid newBid);
        Task<bool> Delete(Bid bidToDelete);
        Task<bool> DeleteRange(List<Bid> listBid);
        Task<(bool IsSuccess, string? message, Bid? returnBid)> PlaceBid(int accountId, int auctionId, decimal amount);
    }
}
