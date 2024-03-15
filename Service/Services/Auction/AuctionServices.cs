using Org.BouncyCastle.Bcpg;
using Repository.Database.Model.AuctionRelated;
using Repository.Database.Model.Enum;
using Repository.Interfaces.AppAccount;
using Repository.Interfaces.Auction;
using Repository.Interfaces.DbTransaction;
using Repository.Interfaces.RealEstate;
using Service.MyHub.HubServices;
using Service.Services.AppAccount;
using Service.Services.RealEstate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Service.Services.Auction
{
	public class AuctionServices
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly AuctionHubService _auctionHubService;

		public AuctionServices(IUnitOfWork unitOfWork, AuctionHubService auctionBidSeevice)
		{
			_unitOfWork = unitOfWork;
			_auctionHubService = auctionBidSeevice;
		}

		public async Task<bool> CheckForJoinedAuction(int accountId, int auctionId)
		{
			return await _unitOfWork.Repositories.joinedAuctionRepository.IsJoined(accountId, auctionId);
		}
		public async Task<List<Repository.Database.Model.AuctionRelated.Auction>> GetActiveAuctions()
		{
			return await _unitOfWork.Repositories.auctionRepository.GetActiveAuctions();
		}
		public async Task<Repository.Database.Model.AuctionRelated.Auction> GetById(int id)
		{
			return await _unitOfWork.Repositories.auctionRepository.GetAsync(id);
		}
		public async Task<List<Repository.Database.Model.AuctionRelated.Auction>> GetByCompanyId(int companyId)
		{
			var getEstates = (await _unitOfWork.Repositories.estateRepository.GetByCompanyId(companyId)).Select(e => e.EstateId).ToArray();
			if (getEstates is null)
				throw new Exception("something wrong with get estate, it should never be null, only empty list or so");
			var result = new List<Repository.Database.Model.AuctionRelated.Auction>();
			foreach (var estate in getEstates)
			{
				var tryGetAuctions = await _unitOfWork.Repositories.auctionRepository.GetByEstateId(estate);
				if (tryGetAuctions is not null)
					result = result.Concat(tryGetAuctions).ToList();
			}
			return result;
		}
		public async Task<List<Repository.Database.Model.AuctionRelated.Auction>> GetAll()
		{
			return await _unitOfWork.Repositories.auctionRepository.GetAllAsync();
		}
		public async Task<List<Repository.Database.Model.AuctionRelated.Auction>> GetRange(int start, int amount)
		{
			if (start < 0 || amount <= 0)
				throw new ArgumentException("start, amount must > 0");
			return await _unitOfWork.Repositories.auctionRepository.GetRange(start, amount);
		}
		public async Task<Repository.Database.Model.AuctionRelated.Auction?> GetInclude(int auctionId, string includes)
		{
			return (await _unitOfWork.Repositories.auctionRepository.GetByCondition(a => a.AuctionId == auctionId, includeProperties: includes)).FirstOrDefault();
		}
		public async Task<List<Repository.Database.Model.AuctionRelated.Auction>> GetRangeInclude_Estate_Company(int start, int amount)
		{
			if (start < 0 || amount <= 0)
				throw new ArgumentException("start, amount must > 0");
			var result = await _unitOfWork.Repositories.auctionRepository.GetRange_IncludeEstate_Company(start, amount);
			foreach (var auction in result)
			{
				var estateId = auction.EstateId;
				auction.Estate.EstateCategory = await _unitOfWork.Repositories.estateCategoriesRepository.GetByEstateId(estateId);
			}
			return result;
		}
		public async Task<List<Repository.Database.Model.AuctionRelated.Auction>> GetAuctionsInclude(Expression<Func<Repository.Database.Model.AuctionRelated.Auction, bool>> expression = null,
			Func<IQueryable<Repository.Database.Model.AuctionRelated.Auction>, IOrderedQueryable<Repository.Database.Model.AuctionRelated.Auction>> orderBy = null,
			string includeProperties = "")
		{
			return await _unitOfWork.Repositories.auctionRepository
				.GetByCondition(expression, orderBy, includeProperties);
		}
		public async Task<List<Repository.Database.Model.AuctionRelated.Auction>> GetByEstateId(int estateId)
		{
			return await _unitOfWork.Repositories.auctionRepository.GetByEstateId(estateId);
		}
		public async Task<List<Repository.Database.Model.AuctionRelated.Auction>> GetWithCondition(
			Expression<Func<Repository.Database.Model.AuctionRelated.Auction, bool>> expression = null,
			Func<IQueryable<Repository.Database.Model.AuctionRelated.Auction>, IOrderedQueryable<Repository.Database.Model.AuctionRelated.Auction>> orderBy = null,
			string includeProperties = "")
		{
			return await _unitOfWork.Repositories.auctionRepository.GetByCondition(expression, orderBy, includeProperties);
		}
		public async Task<(bool IsSuccess, string? message, Repository.Database.Model.AuctionRelated.Auction?)> Create(Repository.Database.Model.AuctionRelated.Auction auction)
		{

			int comparison1 = DateTime.Compare(auction.EndDate, auction.StartDate);
			if (comparison1 <= 0)
			{
				return (false, "EndDate is <= StartDate", null);
			}
			int comparison2 = DateTime.Compare(auction.EndPayDate, auction.EndDate);
			if (comparison2 <= 0)
			{
				return (false, "EndPayDate is <= EndDate", null);
			}
			var getEstate = await _unitOfWork.Repositories.estateRepository.GetAsync(auction.EstateId);
			if (getEstate is not null)
			{
				if(getEstate.Status.Equals(EstateStatus.REMOVED) ||
					getEstate.Status.Equals(EstateStatus.BANNED) ||
					getEstate.Status.Equals(EstateStatus.FINISHED))
				{
					return (false,"Cannot create auction, estate is "+ getEstate.Status.ToString(),null);
				}
				var tryGetEstateAuctionStatus = await _unitOfWork.Repositories.auctionRepository.GetByEstateId(getEstate.EstateId);
				//check status cua tung auction, neu co cai nao != CANCELLD && != FAILED_TO_PAY ( tuc la vd: SUCCESS, ONGOING, NOT_STARTEED_PENDING )... ==> ko hop li de tao moi
				foreach (var auc in tryGetEstateAuctionStatus)
				{
					if (auc.Status != Repository.Database.Model.Enum.AuctionStatus.CANCELLED &&
						auc.Status != Repository.Database.Model.Enum.AuctionStatus.FAILED_TO_PAY) // nghia la auction cho mieng dat nay chua bi huy, neu v thi ko duoc tao auction moi cho no
					{
                        return (false, "error in creeate auction, this is because the estate you select has already been in auction", null);
                    }
                }
			}


			var getCompanyAccount = await _unitOfWork.Repositories.accountRepository.GetAsync(getEstate.CompanyId);
			if(getCompanyAccount.Balance <= 0)
			{
				return (false, "cannot create new aucction your balance is under 0, fill your wallet now",null);
			}



			var createResult = await _unitOfWork.Repositories.auctionRepository.CreateAsync(auction);
			if (createResult is not null)
			{
				//SIGNALR
				await _auctionHubService.CreateAuctionSuccess(createResult);
				return (true, "success", createResult);
			}
			else
			{
				return (false, "Fail to create", null);
			}
		}
		public async Task<(bool IsSuccess, string? message)> CancelAuction(Repository.Database.Model.AuctionRelated.Auction auction)
		{
			try
			{
				var getEstate = await _unitOfWork.Repositories.estateRepository.GetAsync(auction.EstateId);
				await _unitOfWork.BeginTransaction();
				var startTime = auction.StartDate;
				var endTime = auction.EndDate;
				var status = auction.Status;
				//if (DateTime.Compare(DateTime.Now, endTime) >= 0)
				//{
				//	throw new Exception("over time to cancel");
				//}
				if (status.Equals(AuctionStatus.SUCCESS) ||
					status.Equals(AuctionStatus.CANCELLED) ||
					status.Equals(AuctionStatus.FAILED_TO_PAY)) 
				{
					throw new Exception("the status is not valid to cancel or you have already cancelled, status now is: " + status.ToString());
				}
				//auction.Status = AuctionStatus.CANCELLED;
				///
				if (status.Equals(AuctionStatus.PENDING_PAYMENT))
				{
					// tra lai tien entrence fee cua company cho customer
					var getReceipt = await _unitOfWork.Repositories.auctionReceiptRepository.GetAsync(auction.ReceiptId.Value);// receipt id Ko the null, do khi win thi no da set ReceiptId roi
					if (getReceipt is null)
					{
						throw new Exception("with status " + status + " the receipt can never be null, something really wrong");
					}
					// tru tien entrencee fee da lay ngay luc thang cua compnay
					var getCompany = await _unitOfWork.Repositories.accountRepository.GetAsync(getEstate.CompanyId);
					getCompany.Balance -= auction.EntranceFee;
					// tra la het tien ( ke ca entrence fee ) cho customer do day la cancel boi Compnay ( bad type )
					var getWinner = await _unitOfWork.Repositories.accountRepository.GetAsync(getReceipt.BuyerId.Value);
					var amountUserHavePaid = getReceipt.Amount - getReceipt.RemainAmount;
					getWinner.Balance += amountUserHavePaid;
					// update lai company, buyder balance, xoa receipt
					var update1 = await _unitOfWork.Repositories.accountRepository.UpdateAsync(getWinner);
					var update2 = await _unitOfWork.Repositories.accountRepository.UpdateAsync(getCompany);


					// HIEN KO NEN XOA RECEIPT, do receipt app phai an tien 
					//var update3 = await _unitOfWork.Repositories.auctionReceiptRepository.DeleteAsync(getReceipt);
					////////////////////////////////////////////////////////////////////


					auction.Status = AuctionStatus.CANCELLED;
					var update4 = await _unitOfWork.Repositories.auctionRepository.UpdateAsync(auction);
					if (update1 is false || update2 is false  || update4 is false )//|| update3 is false)
					{
						throw new Exception("Error in update cancel auction");
					}
					else
					{
						await _unitOfWork.SaveChangesAsync();
						await _unitOfWork.CommitAsync();
						//SIGNALR
						await _auctionHubService.UpdateAuctionSuccess(auction);
						return (true, "success");
					}
				}
				else // NOT_STARTED, ONGOING
				{
					var joinedAccounts = await _unitOfWork.Repositories.joinedAuctionRepository
					.GetByCondition(j => j.AuctionId == auction.AuctionId, includeProperties: "Account");//auction.JoinedAccounts;
																										 // do shit 
																										 // Hoàn lại tiền Entrence Fee cho mọi nguời trong JoinedAuction có Status là REGISTERD 
					foreach (var joinedOne in joinedAccounts)
					{
						// neu nguoi do da quit hoac bi banned thi ko hoan tien, quit thi hoan tien luc quit roi 
						if (joinedOne.Status.Equals(JoinedAuctionStatus.QUIT) is true ||
							joinedOne.Status.Equals(JoinedAuctionStatus.BANNED) is true)
						{
							continue;
						}
						var account = joinedOne.Account;
						account.Balance += auction.EntranceFee;
						await _unitOfWork.Repositories.accountRepository.UpdateAsync(account);
					}
					// do shit 
					//if (auction.Status.Equals(AuctionStatus.NOT_STARTED) is false &&
					//auction.Status.Equals(AuctionStatus.ONGOING) is false)
					//{
					//	throw new Exception("the status is not valid to cancel or you have already cancelled");
					//}
					auction.Status = AuctionStatus.CANCELLED;
					var result = await _unitOfWork.Repositories.auctionRepository.UpdateAsync(auction);
					if (result)
					{
						await _unitOfWork.SaveChangesAsync();
						await _unitOfWork.CommitAsync();
						//SIGNALR
						await _auctionHubService.UpdateAuctionSuccess(auction);
						return (true, "success");
					}
				}
				throw new Exception("error");
			}
			catch (Exception ex)
			{
				await _unitOfWork.RollBackAsync();
				return (false, ex.Message);
			}
		}
		public async Task<(bool IsSuccess, string? message)> AdminForceCancelAuction(Repository.Database.Model.AuctionRelated.Auction auction)
		{
			//await _unitOfWork.BeginTransaction();
			try
			{
				var estate = await _unitOfWork.Repositories.estateRepository.GetAsync(auction.EstateId);
				var status = auction.Status;
				if (status.Equals(AuctionStatus.SUCCESS) // 3 th còn lại được cái trên lo 
					//status.Equals(AuctionStatus.PENDING_PAYMENT) ||
					//status.Equals(AuctionStatus.CANCELLED)
					)
				{
					throw new Exception("auction has finished, cannot cancel");
					//await _unitOfWork.BeginTransaction();
					//switch (status)
					//{

					//	case AuctionStatus.SUCCESS:
					//		var getReceipt_2 = (await _unitOfWork.Repositories.auctionReceiptRepository.GetByCondition(a => a.AuctionId == auction.AuctionId)).FirstOrDefault();
					//		if (getReceipt_2 is null)
					//		{
					//			await _unitOfWork.RollBackAsync();
					//			throw new Exception("with status " + status + " the receipt can never be null, something really wrong");
					//		}
					//		int companyID = (await _unitOfWork.Repositories.estateRepository.GetAsync(auction.EstateId)).CompanyId;

					//		var amountUserHavePaid_2 = getReceipt_2.Amount;
					//		var amountCompanyHaveTaken_2 = getReceipt_2.Amount - getReceipt_2.Commission;

					//		var userAccount_2 = await _unitOfWork.Repositories.accountRepository.GetAsync(getReceipt_2.BuyerId.Value);
					//		var companyAccount_2 = await _unitOfWork.Repositories.accountRepository.GetAsync(companyID);

					//		userAccount_2.Balance += amountUserHavePaid_2;
					//		companyAccount_2.Balance -= amountCompanyHaveTaken_2;
					//		///
					//		/// NOTE , neeu balance xuong 0  thi company do co van de, admin tu lam viec lai
					//		///
					//		var update1_2 = await _unitOfWork.Repositories.accountRepository.UpdateAsync(userAccount_2);
					//		var update2_2 = await _unitOfWork.Repositories.auctionReceiptRepository.DeleteAsync(getReceipt_2);
					//		auction.Status = AuctionStatus.CANCELLED;
					//		var update3_2 = await _unitOfWork.Repositories.auctionRepository.UpdateAsync(auction);
					//		var update4_2 = await _unitOfWork.Repositories.accountRepository.UpdateAsync(companyAccount_2);
					//		if (update1_2 is false || update2_2 is false || update3_2 is false || update4_2 is false)
					//		{
					//			await _unitOfWork.RollBackAsync();
					//			throw new Exception("Error in update admin cancel auction");
					//		}
					//		break;
					//	default:
					//		await _unitOfWork.RollBackAsync();
					//		throw new Exception("No need to cancel, it has already been cancelled");
					//}
					//await _unitOfWork.SaveChangesAsync();
					//await _unitOfWork.CommitAsync();
					////SIGNALR
					//await _auctionHubService.UpdateAuctionSuccess(auction);
				}
				else
				{
					var result = await CancelAuction(auction);
					if (result.IsSuccess == false)
					{
						throw new Exception(result.message);
					}
				}
				return (true, "Success");
			}
			catch (Exception ex)
			{
				return (false, ex.Message);
			}

		}
		public async Task<bool> Update(Repository.Database.Model.AuctionRelated.Auction auction)
		{

			var result = await _unitOfWork.Repositories.auctionRepository.UpdateAsync(auction);
			if (result)
			{
				return result;
			}
			else
			{
				throw new Exception("Fail to Update");
			}
		}
	}
}

