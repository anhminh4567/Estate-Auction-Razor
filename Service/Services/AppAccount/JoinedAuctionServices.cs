using Repository.Database.Model.AppAccount;
using Repository.Implementation.AppAccount;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Service.Services.Auction;
using Repository.Interfaces.AppAccount;
using Repository.Database.Model.Enum;

namespace Service.Services.AppAccount
{
    public class JoinedAuctionServices
    {
        private readonly IJoinedAuctionRepository _joinedAuctionRepository;
        public JoinedAuctionServices(IJoinedAuctionRepository joinedAuctionRepository)
        {
            _joinedAuctionRepository = joinedAuctionRepository;
        }
        public async Task<List<JoinedAuction>> GetByAuctionId(int auctionId) 
        {
            return await _joinedAuctionRepository.GetByCondition(j => j.AuctionId == auctionId);
        }
        public async Task<List<JoinedAuction>> GetByAuctionId_Status(int auctionId, JoinedAuctionStatus status)
        {
            return await _joinedAuctionRepository.GetByCondition(j => j.AuctionId == auctionId && j.Status.Equals(status));
        }
        public async Task<List<JoinedAuction>> GetByAccountId(int accountId)
        {
            return await _joinedAuctionRepository.GetByCondition(j => j.AccountId == accountId);
        }
        public async Task<JoinedAuction> Create(JoinedAuction joinedAuction) 
        {
            return await _joinedAuctionRepository.CreateAsync(joinedAuction);
        }
        public async Task<bool> DeleteRange(List<JoinedAuction> joinedAuctions) 
        {
            return await _joinedAuctionRepository.DeleteRange(joinedAuctions);   
        }
        public async Task<bool> Update(JoinedAuction joinedAuctions)
        {
            return await _joinedAuctionRepository.UpdateAsync(joinedAuctions);
        }
        public async Task<bool> IsJoined(int accountId, int auctionId)
        {
            return await _joinedAuctionRepository.IsJoined(accountId, auctionId);
        }
    }
}
