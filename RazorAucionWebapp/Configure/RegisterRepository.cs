using Repository.Implementation.AppAccount;
using Repository.Implementation.Auction;
using Repository.Implementation.RealEstate;
using Repository.Implementation;
using Repository.Interfaces.Auction;
using Repository.Interfaces.RealEstate;
using Repository.Interfaces;

namespace RazorAucionWebapp.Configure
{
	public static class RegisterRepository
	{
		public static IServiceCollection AddMyRepositories(this IServiceCollection services) 
		{
			services.AddScoped<IAccountRepository, AccountRepository>();
			services.AddScoped<IAccountImageRepository, AccountImageRepository>();
			services.AddScoped<ICompanyRepository, CompanyRepository>();

			services.AddScoped<IAuctionRepository, AuctionRepository>();
			services.AddScoped<IAuctionReceiptRepository, AuctionReceiptRepository>();
			services.AddScoped<IBidRepository, BidRepository>();

			services.AddScoped<IEstateRepository, EstateRepository>();
			services.AddScoped<IEstateCategoriesRepository, EstateCategoriesRepository>();
			services.AddScoped<IEstateCategoryDetailRepository, EstateCategoryDetailRepository>();
			services.AddScoped<IEstateImagesRepository, EstateImagesRepository>();

			services.AddScoped<ITransactionRepository, TransactionRepository>();
			services.AddScoped<IImagesRepository, ImageRepository>();
			return services;
		}
	}
}
