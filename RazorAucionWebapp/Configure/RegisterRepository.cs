using Repository.Implementation.AppAccount;
using Repository.Implementation.Auction;
using Repository.Implementation.RealEstate;
using Repository.Implementation;
using Repository.Interfaces.Auction;
using Repository.Interfaces.RealEstate;
using Repository.Interfaces;
using Repository.Implementation.DbTransaction;
using Repository.Interfaces.DbTransaction;

namespace RazorAucionWebapp.Configure
{
	public static class RegisterRepository
	{
		public static IServiceCollection AddMyRepositories(this IServiceCollection services) 
		{
            services.AddScoped<IAccountRepository, AccountRepository>();
            services.AddScoped<IAccountImageRepository, AccountImageRepository>();
            services.AddScoped<ICompanyRepository, CompanyRepository>();

            services.AddScoped<IJoinedAuctionRepository, JoinedAuctionRepository>();
            services.AddScoped<IAuctionRepository, AuctionRepository>();
            services.AddScoped<IAuctionReceiptRepository, AuctionReceiptRepository>();
            services.AddScoped<IAuctionReceiptPaymentRepository, AuctionReceiptPaymentRepository>();
            services.AddScoped<IBidRepository, BidRepository>();

            services.AddScoped<IEstateRepository, EstateRepository>();
            services.AddScoped<IEstateCategoriesRepository, EstateCategoriesRepository>();
            services.AddScoped<IEstateCategoryDetailRepository, EstateCategoryDetailRepository>();
            services.AddScoped<IEstateImagesRepository, EstateImagesRepository>();

            services.AddScoped<ITransactionRepository, TransactionRepository>();
            services.AddScoped<IImagesRepository, ImageRepository>();

            services.AddScoped<IAuctionReceiptPaymentRepository, AuctionReceiptPaymentRepository>();
			services.AddScoped<INotificationRepository, NotificationRepository>();


			services.AddScoped<IRepositoryWrapper, RepositoryWrapper>();

            
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            return services;
		}
	}
}
