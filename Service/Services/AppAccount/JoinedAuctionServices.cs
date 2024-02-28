using Repository.Database.Model.AppAccount;
using Repository.Implementation.AppAccount;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Service.Services.Auction;
using Repository.Interfaces.AppAccount;

namespace Service.Services.AppAccount
{
    public class JoinedAuctionServices
    {
        private readonly IJoinedAuctionRepository _joinedAuctionRepository;
        public JoinedAuctionServices(IJoinedAuctionRepository joinedAuctionRepository)
        {
            _joinedAuctionRepository = joinedAuctionRepository;
        }
        public async Task<bool> IsJoined(int accountId, int auctionId)
        {
            return await _joinedAuctionRepository.IsJoined(accountId, auctionId);
        }
    }
}
