using Repository.Database.Model.AppAccount;
using Repository.Database.Model.AuctionRelated;
using Repository.Database.Model.Enum;
using Repository.Interfaces.AppAccount;
using Repository.Interfaces.Auction;
using Repository.Interfaces.DbTransaction;
using Service.MyHub.HubServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Service.Services.Auction
{
    public class BidServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly BidHubServices _bidHubServices;

		public BidServices(IUnitOfWork unitOfWork, BidHubServices bidHubServices)
		{
			_unitOfWork = unitOfWork;
			_bidHubServices = bidHubServices;
		}


		//private readonly IBidRepository _bidRepository;
		//private readonly IAuctionRepository _auctionRepository;
		//private readonly IAccountRepository _accountRepository;
		//private readonly IJoinedAuctionRepository _joinedAuctionRepository;
		//public BidServices(IBidRepository bidRepository, IAuctionRepository auctionRepository, IAccountRepository accountRepository, IJoinedAuctionRepository joinedAuctionRepositor)
		//{
		//	_bidRepository = bidRepository;
		//	_auctionRepository = auctionRepository;
		//	_accountRepository = accountRepository;
		//	_joinedAuctionRepository = joinedAuctionRepositor;
		//}
		public async Task<List<Bid>> GetByAccountId(int accountId)
        {
            if (accountId == 0)
                throw new ArgumentNullException("argument is wrong");
            return await _unitOfWork.Repositories.bidRepository.GetByCondition(b => b.BidderId == accountId);
        }
        public async Task<List<Bid>> GetByAuctionId(int auctionId)
        {
            if (auctionId == 0)
                throw new ArgumentNullException("argument is wrong");
            return await _unitOfWork.Repositories.bidRepository.GetByCondition(b => b.AuctionId == auctionId);
        }
        public async Task<Bid?> GetHighestBids(int auctionId)
        {
            if (auctionId == 0)
                throw new ArgumentNullException("argument is wrong");
            return (await _unitOfWork.Repositories.bidRepository.GetByCondition(b => b.AuctionId == auctionId, includeProperties: "Bidder,Auction"))
                .OrderByDescending(b => b.Amount)
                .FirstOrDefault();
        }
        public async Task<List<Bid>> GetByAuctionId_AccountId(int auctionId, int accountId)
        {
            if (auctionId == 0 || accountId == 0)
                throw new ArgumentNullException("argument is wrong");
            return await _unitOfWork.Repositories.bidRepository.GetByCondition(b => b.AuctionId == auctionId && b.BidderId == accountId);
        }
        public async Task<Bid> Create(Bid newBid)
        {
            var tryGetAuction = await _unitOfWork.Repositories.auctionRepository.GetAsync(newBid.AuctionId);
            var tryGetAccount = await _unitOfWork.Repositories.accountRepository.GetAsync(newBid.BidderId);
            if (tryGetAuction is null || tryGetAccount is null)
                throw new Exception("auction or account is null");
            var auctionStatus = tryGetAuction.Status;
            var accountStatus = tryGetAccount.Status;
            if (auctionStatus != AuctionStatus.ONGOING || (accountStatus == AccountStatus.ACTIVED) is false)
                throw new Exception("unqualified for bidding, either the auction is not happending or your account is not verified");
            var tryGetJoinedAuction = (await _unitOfWork.Repositories.joinedAuctionRepository.GetByCondition(j => j.AuctionId == newBid.AuctionId && j.AccountId == newBid.BidderId)).FirstOrDefault();
            if (tryGetJoinedAuction is null)
                throw new Exception("this use has not joined the auction yet, please join before placing bid");
            newBid.Time = DateTime.Now;

            var createResult = await _unitOfWork.Repositories.bidRepository.CreateAsync(newBid);
            if (createResult is null)
                throw new Exception("fail to create");
            else
            {
                return createResult;
            }


        }
        public async Task<bool> Delete(Bid bidToDelete)
        {
            return await _unitOfWork.Repositories.bidRepository.DeleteAsync(bidToDelete);
        }
        public async Task<bool> DeleteRange(List<Bid> listBid)
        {
            var result =  await _unitOfWork.Repositories.bidRepository.DeleteRange(listBid);
            await _bidHubServices.DeleteBids(listBid);
            return result;
        }
        public async Task<(bool IsSuccess,string? message, Bid? returnBid)> PlaceBid(int accountId, int auctionId, decimal amount)
        {
            try
            {
                var auction = (await _unitOfWork.Repositories.auctionRepository
                    .GetByCondition(a => a.AuctionId == auctionId,includeProperties: "Bids.Bidder,JoinedAccounts.Account,Estate.Images.Image,Estate.EstateCategory.CategoryDetail"))
                    .FirstOrDefault();
                var auctionBids = auction.Bids.OrderByDescending(b => b.Amount).ToList();
                var highestBid = auctionBids.FirstOrDefault();
                var joinedAccounts = auction.JoinedAccounts
                    //.Select(j => j.Account)
                    .ToList();
                var Winner = (await _unitOfWork.Repositories.auctionReceiptRepository
                    .GetByCondition(a => a.AuctionId == auctionId))
                    .FirstOrDefault();//_auctionReceiptServices.GetByAuctionId(AuctionID);
                if (Winner is not null && auction.Status.Equals(AuctionStatus.ONGOING) == false)
                {
                    return (false, "this auction has already finished, you cannot bid anymore", null);
                }
                // lay account nguoi dung hien taij trong joinedAccounts
                var getJoinedAccount = joinedAccounts?.FirstOrDefault(a => a.AccountId == accountId);
                if (getJoinedAccount is null)
                {
                    return (false, "user has not joined auction yet to bid",null);
                }
                // neu nguoi dung bi bann ==> fail
                if (getJoinedAccount.Status.Equals(JoinedAuctionStatus.BANNED))
                {
                    return (false, "user is banned",null);
                }
                ////////////// HIGHER THAN TOP BID /////////////
                if (amount <= highestBid?.Amount)
                {
                    return (false, "your cannot place a bit lower than the top dog",null);
                }
                ////////////// CHECK IF BID IS VALID AGAINST AUCTION CONDITION /////////////
                if (highestBid is not null)
                {
                    var compareToBidJump = (amount - highestBid?.Amount) >= auction.IncrementPrice;
                    if (compareToBidJump is false)
                    {
                        return (false, "your bid must match increament price condition",null);
                    }
                }
                else
                {
                    var compareToBidJump_FirstBidder = amount >= auction.IncrementPrice;
                    if (compareToBidJump_FirstBidder is false)
                    {
                        return (false, "your bid must match increament price condition",null);
                    }
                }
                ////////////// CHECK IF BID IS VALID AGAINST AUCTION CONDITION /////////////

                ///////////// ADD TO DB /////////////
                var createResult = await _unitOfWork.Repositories.bidRepository.CreateAsync(new Bid()
                {
                    BidderId = accountId,
                    AuctionId = auctionId,
                    Amount = amount,
                    Time = DateTime.Now,
                });
                if (createResult is null)
                {
                    return (false, "cannot create bid, something wrong, try again later",null);
                }
                var getAccountPlacedThisBid = await _unitOfWork.Repositories.accountRepository.GetAsync(createResult.BidderId);

                //SIGNALR 
                await _bidHubServices.SendNewBid(createResult,getAccountPlacedThisBid);

				return (true, "Success", createResult);
            }catch (Exception ex) {
                return (false, ex.Message,null);
            }
        }
    }
}
