using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repository.Database.Model.AppAccount;
using Repository.Database.Model.AuctionRelated;
using Repository.Database.Model.RealEstate;

namespace Service.Interfaces.Auction
{
    public interface IAuctionReceiptPaymentServices
    {
        Task<List<AuctionReceiptPayment>?> GetByReceiptId(int receiptId, string includeProperties = null);
        Task<List<AuctionReceiptPayment>?> GetByAccountId_ReceiptId(int accId, int receiptId);
        Task<AuctionReceiptPayment?> Create(AuctionReceiptPayment auctionReceiptPayment);
        Task<bool> Update(AuctionReceiptPayment auctionReceiptPayment);
        Task<bool> Delete(AuctionReceiptPayment auctionReceiptPayment);
        Task<bool> DeleteRange(List<AuctionReceiptPayment> auctionReceiptPayments);
        Task<(bool IsSuccess, string? message)> CreateAuctionReceiptPayment(
                    Account userAccount,
                    Account CompanyAccount,
                    Repository.Database.Model.AuctionRelated.Auction auction,
                    Estate esate,
                    AuctionReceipt auctionReceipt,
                    List<AuctionReceiptPayment> auctionReceiptPayment,
                    decimal PayAmount,
                    decimal commissionPercentage,
                    decimal commissionFixedPrice);
    }
}
