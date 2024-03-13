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
        public async Task<(bool IsValid, string message)> CheckIfUserIsQualifiedToJoin(Account account, Repository.Database.Model.AuctionRelated.Auction auction) //supplement the above function
        {
            if (account.Role.Equals(Role.CUSTOMER) is false)
                return ( false, "not a customer");
            // check xem co JoinedAuction cua account do ko ( BUT neu no QUIT hay baneed thi no van xem la da tham gia roi )
            if ((await CheckIfUserHasJoinedTheAuction(account, auction)) is false)
                return (true, "user havee not joined before");
            var aucStatus = auction.Status;
            if (aucStatus.Equals(AuctionStatus.CANCELLED) ||
                aucStatus.Equals(AuctionStatus.PENDING_PAYMENT) ||
                aucStatus.Equals(AuctionStatus.SUCCESS) ||
				aucStatus.Equals(AuctionStatus.FAILED_TO_PAY))
				return (false," auction is already over");
            //CHECK neu nguoi do bi ban hay quit 
            var getUserJoinedAuction = await GetByAccountId_AuctionId(account.AccountId, auction.AuctionId);
            if (getUserJoinedAuction.Status.Equals(JoinedAuctionStatus.BANNED) || getUserJoinedAuction.Status.Equals(JoinedAuctionStatus.REGISTERED))
                return (false, "user has already joined or banned");
            return (true,"user is valid, but he has joiend before");
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
                // Check xem user co du tien hay ko 
                var isBalanceEnough = (userBalance >= getAuction.EntranceFee);
                if (isBalanceEnough)
                {
                    // CHI CO THE THAM GIA NEU AuctionStauts = NOT_STARTED ( ON_GOING cung ko duoc, ON_GOING chi co the quit auction ) 
                    if (getAuction.Status.Equals(AuctionStatus.ONGOING) == false &&
                        getAuction.Status.Equals(AuctionStatus.NOT_STARTED) == false)
                    {
                        return (false, "auction is happening");
                    }
                    // Tong so nguoi tham gia > max ==> ko tham gia duoc nua
                    if (getAuction.JoinedAccounts.Count >= getAuction.MaxParticipant)
                    {
                        return (false, "reach max participant");
                    }
                    var isValidToJoin = await CheckIfUserIsQualifiedToJoin(getUser, getAuction);
                    if (isValidToJoin.IsValid == false)
                    {
                        return (false, isValidToJoin.message);
                    }
                    if(isValidToJoin.IsValid == true) // user chua join auction
                    {
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
							Status = JoinedAuctionStatus.REGISTERED
						};
						var createResult = await _unitOfWork.Repositories.joinedAuctionRepository.CreateAsync(newUserJoined);
						if (createResult is null)
							throw new Exception("someting wrong wiith create JoinedAuction, try again");
						await _unitOfWork.SaveChangesAsync();
						await _unitOfWork.CommitAsync();
                    }
                    else
                    {
                        return (false, isValidToJoin.message);
                    }
                   
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
                        
                        // neu AUCTION chua dien ra (NOT_STARTED)  ma quit thi hoan tien || neu ( ONGOING ) ==> Ko hoan tien
                        if(getAuction.Status.Equals(AuctionStatus.NOT_STARTED))
                        {
							getUser.Balance += getAuction.EntranceFee;
							await _unitOfWork.Repositories.accountRepository.UpdateAsync(getUser);// _accountServices.Update(getUser);
                        }
                        else // Neeu AUCTION dang dien ra ( ONGOING ) ==> Ko hoan tien, tien vo tai khoan company
                        {
                            var getEstate = await _unitOfWork.Repositories.estateRepository.GetAsync(getAuction.EstateId);
                            var getCompany = await _unitOfWork.Repositories.accountRepository.GetAsync(getEstate.CompanyId);
                            getCompany.Balance += getAuction.EntranceFee;
                        }
                        
                        // REMOVE ALL BID OF THE USER IF EXIST
                        if (getBids is not null && getBids.Count > 0)
                        {
                            await _unitOfWork.Repositories.bidRepository.DeleteRange(getBids);
                        }
						// Xoa joinedAuction cua nguoi do 
						await _unitOfWork.Repositories.joinedAuctionRepository.DeleteAsync(joinedAuction); //_joinedAuctionServices.DeleteRange(new List<JoinedAuction>() { JoinedAuction });

                        
                        await _unitOfWork.SaveChangesAsync();
                        await _unitOfWork.CommitAsync();

                        return (true, "Success");
                    }
                    else
                    {
                        return (false, "cannot quit , user status is not valid, user status is " + joinedAuction.Status);
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
