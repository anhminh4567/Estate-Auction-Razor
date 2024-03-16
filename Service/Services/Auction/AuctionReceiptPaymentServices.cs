using Repository.Database.Model.AppAccount;
using Repository.Database.Model.AuctionRelated;
using Repository.Database.Model.Enum;
using Repository.Database.Model.RealEstate;
using Repository.Interfaces.Auction;
using Repository.Interfaces.DbTransaction;
using Service.Interfaces.Auction;
using Service.Services.AppAccount;
using Service.Services.RealEstate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services.Auction
{
	public class AuctionReceiptPaymentServices : IAuctionReceiptPaymentServices
	{
		private readonly IUnitOfWork _unitOfWork;
        public AuctionReceiptPaymentServices(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        //private readonly IAuctionReceiptPaymentRepository _auctionReceiptPaymentRepository;

        //public AuctionReceiptPaymentServices(IAuctionReceiptPaymentRepository auctionReceiptPaymentRepository)
        //{
        //	_auctionReceiptPaymentRepository = auctionReceiptPaymentRepository;
        //}

        public async Task<List<AuctionReceiptPayment>?> GetByReceiptId(int receiptId, string includeProperties = null)
		{
			if(includeProperties is null)
				return (await _unitOfWork.Repositories.auctionReceiptPaymentRepository.GetByCondition(a => a.ReceiptId == receiptId));
				var result = (await _unitOfWork.Repositories.auctionReceiptPaymentRepository.GetByCondition(a => a.ReceiptId == receiptId, includeProperties: includeProperties));
			return result;
		}
		public async Task<List<AuctionReceiptPayment>?> GetByAccountId_ReceiptId(int accId, int receiptId)
		{
			return (await _unitOfWork.Repositories.auctionReceiptPaymentRepository.GetByCondition(a => a.AccountId == accId && a.ReceiptId == receiptId)).ToList();
		}
		public async Task<AuctionReceiptPayment?> Create(AuctionReceiptPayment auctionReceiptPayment)
		{
			return await _unitOfWork.Repositories.auctionReceiptPaymentRepository.CreateAsync(auctionReceiptPayment);
		}
		public async Task<bool> Update(AuctionReceiptPayment auctionReceiptPayment)
		{
			return await _unitOfWork.Repositories.auctionReceiptPaymentRepository.UpdateAsync(auctionReceiptPayment);
		}
		public async Task<bool> Delete(AuctionReceiptPayment auctionReceiptPayment)
		{
			return await _unitOfWork.Repositories.auctionReceiptPaymentRepository.DeleteAsync(auctionReceiptPayment);
		}
        public async Task<bool> DeleteRange(List<AuctionReceiptPayment> auctionReceiptPayments)
        {
            return await _unitOfWork.Repositories.auctionReceiptPaymentRepository.DeleteRange(auctionReceiptPayments);
        }
        //public List<AuctionReceiptPayment> AuctionReceiptPayment { get; set; } = default!;
        //public AuctionReceipt AuctionReceipt { get; set; }
        //public Account UserAccount { get; set; }
        //public Auction Auction { get; set; }
        //public Estate Estate { get; set; }
        //public Account Company { get; set; }
        public async Task<(bool IsSuccess, string? message)> CreateAuctionReceiptPayment(
            Account userAccount,
            Account CompanyAccount,
            Repository.Database.Model.AuctionRelated.Auction auction,
            Estate esate,
            AuctionReceipt auctionReceipt,
            List<AuctionReceiptPayment> auctionReceiptPayment,
            decimal PayAmount,
            decimal commissionPercentage,
            decimal commissionFixedPrice)
		{
            if(commissionPercentage <= 0) 
            {
                return (false, "pass the commission from binding appsetting");
            }
            var userBalance = userAccount.Balance;
            var moneyToPayRemain = auctionReceipt.RemainAmount;
            var payEndDate = auction.EndPayDate;
            if (auction.Status.Equals(AuctionStatus.FAILED_TO_PAY) ||
                auction.Status.Equals(AuctionStatus.SUCCESS))
            {
                return (false, "Auction is finished");
            }
            if (PayAmount > userBalance)
            {
                return (false, "You dont have enough balancee");
            }
            if (PayAmount <= 0 || PayAmount > moneyToPayRemain) // if the money you are paying is moree than you need to pay
            {
                return (false, "your money must > 0 and <= " + moneyToPayRemain.ToString());
            }
            if (DateTime.Compare(DateTime.Now, payEndDate) >= 0)
            {
                return (false, "Your pay Time is over");
            }
            try
            {
                await _unitOfWork.BeginTransaction();
                var newPayment = new AuctionReceiptPayment()
                {
                    AccountId = userAccount.AccountId,
                    ReceiptId = auctionReceipt.ReceiptId,
                    PayAmount = PayAmount,
                    PayTime = DateTime.Now,
                };
                /////////// CREATE ///////////
                var createResult = await Create(newPayment);
                if (createResult is null)
                {
                    throw new Exception("cannot create right now, try again later");
                }
                /////////// UPDATE OLD RECEIPT ///////////
                auctionReceipt.RemainAmount -= createResult.PayAmount;// update the receipt remain to pay
                var updateReceiptResult = await _unitOfWork.Repositories.auctionReceiptRepository.UpdateAsync(auctionReceipt); //_auctionReceiptServices.Update(AuctionReceipt);
                if (updateReceiptResult is false)
                {
                    throw new Exception("fail to update receipt try again later");
                }
                /////////// UPDATE BALANCE///////////
                userAccount.Balance -= createResult.PayAmount;// update the receipt remain to pay
                var updateBalanceResult = await _unitOfWork.Repositories.accountRepository.UpdateAsync(userAccount);
                if (updateBalanceResult is false)
                {
                    throw new Exception("fail to update balance try again later");
                }
                /////////// UPDATE If NO REMAIN AMOUNT///////////
                if (auctionReceipt.RemainAmount == 0)
                {
                    /////////// UPDATE APP COMISSION///////////
                    //auctionReceipt.Commission = auctionReceipt.Amount * (commissionPercentage / 100);
                    //auctionReceipt.Commission = commissionFixedPrice;
                    //var update1 = await _unitOfWork.Repositories.auctionReceiptRepository.UpdateAsync(auctionReceipt);


                    auction.Status =AuctionStatus.SUCCESS;
                    esate.Status = EstateStatus.FINISHED;
                    /////////// UPDATE Company BALANCE///////////
                    CompanyAccount.Balance += auctionReceipt.Amount;//- auctionReceipt.Commission;
                    var update2 = await _unitOfWork.Repositories.accountRepository.UpdateAsync(CompanyAccount);
                    var update3 = await _unitOfWork.Repositories.auctionRepository.UpdateAsync(auction);
                    var update4 = await _unitOfWork.Repositories.estateRepository.UpdateAsync(esate);
                    if( update2 is false || update3 is false || update4 is false)//update1 is false ||
					{
                        throw new Exception("fail to update in CreateReceptPayment");
                    }
                }
                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitAsync();
                return (true, "Success");
            }
            catch (Exception ex) 
            {
                await _unitOfWork.RollBackAsync();
                return (false,ex.Message);
            }
           
            
        }
    }
}
