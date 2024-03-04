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

namespace Service.Services.AppAccount
{
	public class JoinedAuctionServices
	{
		private readonly IJoinedAuctionRepository _joinedAuctionRepository;
		public JoinedAuctionServices(IJoinedAuctionRepository joinedAuctionRepository)
		{
			_joinedAuctionRepository = joinedAuctionRepository;
		}
		public async Task<List<JoinedAuction>> GetByAuctionId(int auctionId, bool isInclude = false, string includeProperties = null)
		{
			if (isInclude && string.IsNullOrEmpty(includeProperties) == false)
				return await _joinedAuctionRepository.GetByCondition(j => j.AuctionId == auctionId, includeProperties: includeProperties);
			else
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
		public async Task<JoinedAuction?> GetByAccountId_AuctionId(int accountId, int auctionId)
		{
			return (await _joinedAuctionRepository.GetByCondition(j => j.AuctionId == auctionId && j.AccountId == accountId)).FirstOrDefault();
		}
		private async Task<bool> CheckIfUserHasJoinedTheAuction(Account account, Repository.Database.Model.AuctionRelated.Auction auction)
		{
			var getUserJoinedAuctionJoined = (await _joinedAuctionRepository
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
