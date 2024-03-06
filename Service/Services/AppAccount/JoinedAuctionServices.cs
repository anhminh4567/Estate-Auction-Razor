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
using Repository.Database.Model.AuctionRelated;
using Repository.Interfaces.DbTransaction;
using System.Diagnostics.Contracts;

namespace Service.Services.AppAccount
{
    public class JoinedAuctionServices
    {
        private readonly IUnitOfWork _unitOfWork;

        public JoinedAuctionServices(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        //private readonly IJoinedAuctionRepository _joinedAuctionRepository;
        //public JoinedAuctionServices(IJoinedAuctionRepository joinedAuctionRepository)
        //{
        //	_joinedAuctionRepository = joinedAuctionRepository;
        //}
        public async Task<List<JoinedAuction>> GetByAuctionId(int auctionId, bool isInclude = false, string includeProperties = null)
        {
            if (isInclude && string.IsNullOrEmpty(includeProperties) == false)
                return await _unitOfWork.Repositories.joinedAuctionRepository.GetByCondition(j => j.AuctionId == auctionId, includeProperties: includeProperties);
            else
                return await _unitOfWork.Repositories.joinedAuctionRepository.GetByCondition(j => j.AuctionId == auctionId);
        }
        public async Task<List<JoinedAuction>> GetByAuctionId_Status(int auctionId, JoinedAuctionStatus status)
        {
            return await _unitOfWork.Repositories.joinedAuctionRepository.GetByCondition(j => j.AuctionId == auctionId && j.Status.Equals(status));
        }
        public async Task<List<JoinedAuction>> GetByAccountId(int accountId)
        {
            return await _unitOfWork.Repositories.joinedAuctionRepository.GetByCondition(j => j.AccountId == accountId);
        }
        public async Task<JoinedAuction?> GetByAccountId_AuctionId(int accountId, int auctionId)
        {
            return (await _unitOfWork.Repositories.joinedAuctionRepository.GetByCondition(j => j.AuctionId == auctionId && j.AccountId == accountId)).FirstOrDefault();
        }
        private async Task<bool> CheckIfUserHasJoinedTheAuction(Account account, Repository.Database.Model.AuctionRelated.Auction auction)
        {
            var getUserJoinedAuctionJoined = (await _unitOfWork.Repositories.joinedAuctionRepository
                .GetByCondition(j => j.AuctionId == auction.AuctionId
                    && j.AccountId == account.AccountId))//&& j.Status.Equals(JoinedAuctionStatus.REGISTERED
                .FirstOrDefault();
            if (getUserJoinedAuctionJoined is null)
                return false;
            return true;
        }
        public async Task<bool> CheckIfUserIsQualifiedToJoin(Account account, Repository.Database.Model.AuctionRelated.Auction auction) //supplement the above function
        {
            if (account.Role.Equals(Role.CUSTOMER) is false)
                return false;
            if ((await CheckIfUserHasJoinedTheAuction(account, auction)) is false)
                return true;
            // CHECK USER ACCOUNT STATUS
            // CHECK USER ACCOUNT STATUS
            // CHECK AUCTION STATUS
            // CHECK AUCTION STATUS
            var aucStatus = auction.Status;
            if (aucStatus.Equals(AuctionStatus.CANCELLED) ||
            aucStatus.Equals(AuctionStatus.PENDING_PAYMENT) ||
            aucStatus.Equals(AuctionStatus.SUCCESS))
                return false;
            //CHECK IF YOU ARE BANNED BEFORE
            var getUserJoinedAuction = await GetByAccountId_AuctionId(account.AccountId, auction.AuctionId);
            if (getUserJoinedAuction.Status.Equals(JoinedAuctionStatus.BANNED) || getUserJoinedAuction.Status.Equals(JoinedAuctionStatus.REGISTERED))
                return false;
            return true;
        }
        public async Task<(bool IsSuccess, string? message)> BanUserFromAuction(JoinedAuction joinedAuction)
        {
            try
            {
                var status = joinedAuction.Status;
                if (status.Equals(JoinedAuctionStatus.REGISTERED))// only when user is registered can you ban him, if i quit, or alreay banned, then no
                {
                    await _unitOfWork.BeginTransaction();
                    joinedAuction.Status = JoinedAuctionStatus.BANNED;
                    var updateResult = await _unitOfWork.Repositories.joinedAuctionRepository.UpdateAsync(joinedAuction);// UPDate status to BANNED
                    if (updateResult is false)
                        throw new Exception("cannot update joined auction, check it");
                    var getBids = await _unitOfWork.Repositories.bidRepository
                        .GetByCondition(j => j.AuctionId == joinedAuction.AuctionId && j.BidderId == joinedAuction.AccountId);// REMOVE ALL BID OF THE USER IF EXIST
                    if (getBids.Count > 0)
                    {
                        var deleteResult = await _unitOfWork.Repositories.bidRepository.DeleteRange(getBids);
                        if (deleteResult is false)
                            throw new Exception("cannot delete, check it");
                    }
                    await _unitOfWork.SaveChangesAsync();
                    await _unitOfWork.CommitAsync();
                }
                else
                {
                    return (false, "this user has already been banned or already quit");
                }
                return (true, "success");
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollBackAsync();
                return (false, ex.Message);
            }

        }
        public async Task<JoinedAuction> Create(JoinedAuction joinedAuction)
        {
            return await _unitOfWork.Repositories.joinedAuctionRepository.CreateAsync(joinedAuction);
        }
        public async Task<bool> DeleteRange(List<JoinedAuction> joinedAuctions)
        {
            return await _unitOfWork.Repositories.joinedAuctionRepository.DeleteRange(joinedAuctions);
        }
        public async Task<bool> Update(JoinedAuction joinedAuctions)
        {
            return await _unitOfWork.Repositories.joinedAuctionRepository.UpdateAsync(joinedAuctions);
        }
        public async Task<bool> IsJoined(int accountId, int auctionId)
        {
            return await _unitOfWork.Repositories.joinedAuctionRepository.IsJoined(accountId, auctionId);
        }
        public async Task<(bool IsSuccess, string? message)> JoinAuction(
            int userId,
            int auctionId)
        {
            try
            {
                //await GetJoinAuction(AuctionId);
                var getUser = (await _unitOfWork.Repositories.accountRepository.GetAsync(userId));
                var getAuction = (await _unitOfWork.Repositories.auctionRepository
                    .GetByCondition(a => a.AuctionId == auctionId, includeProperties: "JoinedAccounts"))
                    .FirstOrDefault();
                var userBalance = getUser.Balance;
                var isBalanceEnough = (userBalance >= getAuction.EntranceFee);
                if (isBalanceEnough)
                {
                    if (//Auction.Status.Equals(AuctionStatus.ONGOING) == false &&
                        getAuction.Status.Equals(AuctionStatus.NOT_STARTED) == false)
                    {
                        return (false, "auction is happening");
                    }
                    if (getAuction.JoinedAccounts.Count >= getAuction.MaxParticipant)
                    {
                        return (false, "reach max participant");
                    }
                    var isValidToJoin = await CheckIfUserIsQualifiedToJoin(getUser, getAuction);
                    if (isValidToJoin == false)
                    {
                        return (false, "unqualified condition to join");
                    }

                    await _unitOfWork.BeginTransaction();


                    getUser.Balance -= getAuction.EntranceFee;
                    // Chưa có chuyênr tiền vào company, mới trừ tiền của customer 
                    var updateResult = await _unitOfWork.Repositories.accountRepository.UpdateAsync(getUser);
                    if (updateResult is false)
                        throw new Exception("fail to update user balance");
                    var newUserJoined = new JoinedAuction()
                    {
                        AccountId = getUser.AccountId,
                        AuctionId = getAuction.AuctionId,
                        RegisterDate = DateTime.Now,
                        Status = Repository.Database.Model.Enum.JoinedAuctionStatus.REGISTERED
                    };
                    var createResult = await _unitOfWork.Repositories.joinedAuctionRepository.CreateAsync(newUserJoined);
                    if (createResult is null)
                        throw new Exception("someting wrong wiith create JoinedAuction, try again");
                    await _unitOfWork.SaveChangesAsync();
                    await _unitOfWork.CommitAsync();
                    return (true, "Success");
                }
                else
                {
                    return (false, "not enough balance, put some money in account");
                }
                //return await _unitOfWork.Repositories.joinedAuctionRepository.IsJoined(accountId, auctionId);
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollBackAsync();
                return (false, ex.Message);
            }
        }
        public async Task<(bool IsSuccess, string? message)> QuitAuction(int userId, int auctionId)
        {
            var getUser = (await _unitOfWork.Repositories.accountRepository.GetAsync(userId));
            var getAuction = (await _unitOfWork.Repositories.auctionRepository
                                .GetByCondition(a => a.AuctionId == auctionId, includeProperties: "JoinedAccounts"))
                                .FirstOrDefault();
            var joinedAuction = (await _unitOfWork.Repositories.joinedAuctionRepository
                .GetByCondition(j => j.AuctionId == auctionId && j.AccountId == userId))
                .FirstOrDefault();
            try
            {
                if (getAuction.Status.Equals(AuctionStatus.NOT_STARTED) ||
                getAuction.Status.Equals(AuctionStatus.ONGOING))
                {
                    if (joinedAuction.Status.Equals(JoinedAuctionStatus.REGISTERED))
                    {

                        await _unitOfWork.BeginTransaction();

                        var getBids = await _unitOfWork.Repositories.bidRepository.GetByCondition(b => b.AuctionId == auctionId && b.BidderId == userId);//await _bidServices.GetByAuctionId_AccountId(AuctionId, _userId);
                                                                                                                                                         // REMOVE ALL BID OF THE USER IF EXIST
                        if (getBids is not null && getBids.Count > 0)
                        {
                            await _unitOfWork.Repositories.bidRepository.DeleteRange(getBids);
                        }
                        await _unitOfWork.Repositories.joinedAuctionRepository.DeleteAsync(joinedAuction); //_joinedAuctionServices.DeleteRange(new List<JoinedAuction>() { JoinedAuction });
                        getUser.Balance += getAuction.EntranceFee;
                        await _unitOfWork.Repositories.accountRepository.UpdateAsync(getUser);// _accountServices.Update(getUser);
                        
                        
                        await _unitOfWork.SaveChangesAsync();
                        await _unitOfWork.CommitAsync();

                        return (true, "Success");
                    }
                    else
                    {
                        return (false, "cannot delete, status not valid");
                    }
                }
                else
                {
                    return (false, "cannot delete, status not valid");
                }
            }
            catch (Exception ex) 
            {
                await _unitOfWork.RollBackAsync();
                return (false, ex.Message);
            }
            

        }
    }
}
