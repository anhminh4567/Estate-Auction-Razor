using Repository.Database.Model.AuctionRelated;
using Repository.Interfaces.Auction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services.Auction
{
	public class AuctionReceiptPaymentServices
	{
		private readonly IAuctionReceiptPaymentRepository _auctionReceiptPaymentRepository;

		public AuctionReceiptPaymentServices(IAuctionReceiptPaymentRepository auctionReceiptPaymentRepository)
		{
			_auctionReceiptPaymentRepository = auctionReceiptPaymentRepository;
		}
		public async Task<List<AuctionReceiptPayment>?> GetByReceiptId(int receiptId, string includeProperties = null)
		{
			if(includeProperties is null)
				return (await _auctionReceiptPaymentRepository.GetByCondition(a => a.ReceiptId == receiptId));
				var result = (await _auctionReceiptPaymentRepository.GetByCondition(a => a.ReceiptId == receiptId, includeProperties: includeProperties));
			return result;
		}
		public async Task<List<AuctionReceiptPayment>?> GetByAccountId_ReceiptId(int accId, int receiptId)
		{
			return (await _auctionReceiptPaymentRepository.GetByCondition(a => a.AccountId == accId && a.ReceiptId == receiptId)).ToList();
		}
		public async Task<AuctionReceiptPayment?> Create(AuctionReceiptPayment auctionReceiptPayment)
		{
			return await _auctionReceiptPaymentRepository.CreateAsync(auctionReceiptPayment);
		}
		public async Task<bool> Update(AuctionReceiptPayment auctionReceiptPayment)
		{
			return await _auctionReceiptPaymentRepository.UpdateAsync(auctionReceiptPayment);
		}
		public async Task<bool> Delete(AuctionReceiptPayment auctionReceiptPayment)
		{
			return await _auctionReceiptPaymentRepository.DeleteAsync(auctionReceiptPayment);
		}
        public async Task<bool> DeleteRange(List<AuctionReceiptPayment> auctionReceiptPayments)
        {
            return await _auctionReceiptPaymentRepository.DeleteRange(auctionReceiptPayments);
        }
    }
}
