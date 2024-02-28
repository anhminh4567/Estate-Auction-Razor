using Repository.Database.Model.AppAccount;
using Repository.Implementation.AppAccount;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Service.Services.Auction;

namespace Service.Services.AppAccount
{
    public class JoinedAuctionServices
    {
        private readonly JoinedAuctionRepository _joinedAuctionRepository;
        private readonly AuctionServices _auctionService;
        public JoinedAuctionServices(JoinedAuctionRepository joinedAuctionRepository, AuctionServices auctionService)
        {
            _joinedAuctionRepository = joinedAuctionRepository;
            _auctionService = auctionService;
        }
        public async Task<bool> IsJoined(int accountId, int auctionId)
        {
            return await _joinedAuctionRepository.IsJoined(accountId, auctionId);
        }
        public async Task<Repository.Database.Model.AuctionRelated.Auction> GetAuction(int id)
        {
            return await _auctionService.GetById(id);
        }
    }
}
