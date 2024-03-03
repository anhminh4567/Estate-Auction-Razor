using Repository.Database.Model.AuctionRelated;
using Repository.Database.Model.Enum;
using Repository.Interfaces.AppAccount;
using Repository.Interfaces.Auction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Service.Services.Auction
{
	public class BidServices
	{
		private readonly IBidRepository _bidRepository;
		private readonly IAuctionRepository _auctionRepository;
		private readonly IAccountRepository _accountRepository;
		private readonly IJoinedAuctionRepository _joinedAuctionRepository;
		public BidServices(IBidRepository bidRepository, IAuctionRepository auctionRepository, IAccountRepository accountRepository, IJoinedAuctionRepository joinedAuctionRepositor)
		{
			_bidRepository = bidRepository;
			_auctionRepository = auctionRepository;
			_accountRepository = accountRepository;
			_joinedAuctionRepository = joinedAuctionRepositor;
		}
		public async Task<List<Bid>> GetByAccountId(int accountId)
		{
			if (accountId == 0)
				throw new ArgumentNullException("argument is wrong");
			return await _bidRepository.GetByCondition(b => b.BidderId == accountId);
		}
		public async Task<List<Bid>> GetByAuctionId(int auctionId)
		{
			if(auctionId == 0)
                throw new ArgumentNullException("argument is wrong");
            return await _bidRepository.GetByCondition(b => b.AuctionId == auctionId);
		}
		public async Task<Bid?> GetHighestBids(int auctionId) 
		{
			if(auctionId == 0)
				throw new ArgumentNullException("argument is wrong");
			return (await _bidRepository.GetByCondition(b => b.AuctionId == auctionId, includeProperties: "Bidder,Auction"))
				.OrderByDescending(b => b.Amount)
				.FirstOrDefault();
		}
		public async Task<Bid> Create(Bid newBid)
		{
			var tryGetAuction = await _auctionRepository.GetAsync(newBid.AuctionId);
			var tryGetAccount = await _accountRepository.GetAsync(newBid.BidderId);
			if (tryGetAuction is null || tryGetAccount is null)
				throw new Exception("auction or account is null");
			var auctionStatus = tryGetAuction.Status;
			var accountStatus = tryGetAccount.Status;
			var accountVerified = tryGetAccount.IsVerified == 0 ? false : true;
			if (auctionStatus != AuctionStatus.ONGOING || accountVerified == false || (accountStatus == AccountStatus.ACTIVED) is false )
				throw new Exception("unqualified for bidding, either the auction is not happending or your account is not verified");
            var tryGetJoinedAuction = (await _joinedAuctionRepository.GetByCondition(j => j.AuctionId == newBid.AuctionId && j.AccountId == newBid.BidderId)).FirstOrDefault();
            if (tryGetJoinedAuction is null)
                throw new Exception("this use has not joined the auction yet, please join before placing bid");
			newBid.Time = DateTime.Now;
            return await _bidRepository.CreateAsync(newBid);
		}
		public async Task<bool> Delete(Bid bidToDelete) 
		{
			return await _bidRepository.DeleteAsync(bidToDelete);
		}
	}
}
