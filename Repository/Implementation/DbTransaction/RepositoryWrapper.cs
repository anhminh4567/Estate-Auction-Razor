using Repository.Interfaces;
using Repository.Interfaces.AppAccount;
using Repository.Interfaces.Auction;
using Repository.Interfaces.DbTransaction;
using Repository.Interfaces.RealEstate;

namespace Repository.Implementation.DbTransaction
{
    // this is not necessary
    public class RepositoryWrapper : IRepositoryWrapper
    {
        public IAccountRepository accountRepository { get; private set; }

        public IAccountImageRepository accountImageRepository { get; private set; }

        public ICompanyRepository companyRepository { get; private set; }

        public IJoinedAuctionRepository joinedAuctionRepository { get; private set; }

        public IAuctionRepository auctionRepository { get; private set; }

        public IAuctionReceiptRepository auctionReceiptRepository { get; private set; }

        public IAuctionReceiptPaymentRepository auctionReceiptPaymentRepository { get; private set; }

        public IBidRepository bidRepository { get; private set; }

        public IEstateRepository estateRepository { get; private set; }

        public IEstateCategoryDetailRepository estateCategoryDetailRepository { get; private set; }

        public IEstateCategoriesRepository estateCategoriesRepository { get; private set; }

        public IEstateImagesRepository estateImagesRepository { get; private set; }

        public ITransactionRepository transactionRepository { get; private set; }

        public IImagesRepository imagesRepository { get; private set; }

        public RepositoryWrapper(IAccountRepository accountRepository, IAccountImageRepository accountImageRepository, ICompanyRepository companyRepository, IJoinedAuctionRepository joinedAuctionRepository, IAuctionRepository auctionRepository, IAuctionReceiptRepository auctionReceiptRepository, IAuctionReceiptPaymentRepository auctionReceiptPaymentRepository, IBidRepository bidRepository, IEstateRepository estateRepository, IEstateCategoryDetailRepository estateCategoryDetailRepository, IEstateCategoriesRepository estateCategoriesRepository, IEstateImagesRepository estateImagesRepository, ITransactionRepository transactionRepository, IImagesRepository imagesRepository)
        {
            this.accountRepository = accountRepository;
            this.accountImageRepository = accountImageRepository;
            this.companyRepository = companyRepository;
            this.joinedAuctionRepository = joinedAuctionRepository;
            this.auctionRepository = auctionRepository;
            this.auctionReceiptRepository = auctionReceiptRepository;
            this.auctionReceiptPaymentRepository = auctionReceiptPaymentRepository;
            this.bidRepository = bidRepository;
            this.estateRepository = estateRepository;
            this.estateCategoryDetailRepository = estateCategoryDetailRepository;
            this.estateCategoriesRepository = estateCategoriesRepository;
            this.estateImagesRepository = estateImagesRepository;
            this.transactionRepository = transactionRepository;
            this.imagesRepository = imagesRepository;
        }
    }
}
