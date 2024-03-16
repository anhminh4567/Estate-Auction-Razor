using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repository.Database.Model.AppAccount;
using Repository.Database.Model.Enum;

namespace Service.Interfaces.AppAccount
{
    public interface IJoinedAuctionServices
    {
        Task<List<JoinedAuction>> GetByAuctionId(int auctionId, bool isInclude = false, string includeProperties = null);
        Task<List<JoinedAuction>> GetByAuctionId_Status(int auctionId, JoinedAuctionStatus status);
        Task<List<JoinedAuction>> GetByAccountId(int accountId);
        Task<JoinedAuction?> GetByAccountId_AuctionId(int accountId, int auctionId);
        Task<List<JoinedAuction>?> GetInclude(int auctionId, string included);
        Task<bool> CheckIfUserHasJoinedTheAuction(Account account, Repository.Database.Model.AuctionRelated.Auction auction);
        Task<(bool IsValid, string message)> CheckIfUserIsQualifiedToJoin(Account account, Repository.Database.Model.AuctionRelated.Auction auction);
        Task<(bool IsSuccess, string? message)> BanUserFromAuction(JoinedAuction joinedAuction);
        Task<JoinedAuction> Create(JoinedAuction joinedAuction);
        Task<bool> DeleteRange(List<JoinedAuction> joinedAuctions);
        Task<bool> Update(JoinedAuction joinedAuctions);
        Task<bool> IsJoined(int accountId, int auctionId);
        Task<(bool IsSuccess, string? message)> JoinAuction(
                    int userId,
                    int auctionId);
        Task<(bool IsSuccess, string? message)> QuitAuction(int userId, int auctionId);
    }
}
