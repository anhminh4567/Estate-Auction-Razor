using Repository.Interfaces.AppAccount;
using Repository.Interfaces.Auction;
using Repository.Interfaces.RealEstate;

namespace Repository.Interfaces.DbTransaction
{
    public interface IRepositoryWrapper
    {
        IAccountRepository accountRepository { get; }
        IAccountImageRepository accountImageRepository { get; }
        ICompanyRepository companyRepository { get; }
        IJoinedAuctionRepository joinedAuctionRepository { get; }
        IAuctionRepository auctionRepository { get; }
        IAuctionReceiptRepository auctionReceiptRepository { get; }
        IAuctionReceiptPaymentRepository auctionReceiptPaymentRepository { get; }

        IBidRepository bidRepository { get; }
        IEstateRepository estateRepository { get; }

        IEstateCategoryDetailRepository estateCategoryDetailRepository { get; }
        IEstateCategoriesRepository estateCategoriesRepository { get; }
        IEstateImagesRepository estateImagesRepository { get; }

        ITransactionRepository transactionRepository { get; }
        IImagesRepository imagesRepository { get; }

    }
}
