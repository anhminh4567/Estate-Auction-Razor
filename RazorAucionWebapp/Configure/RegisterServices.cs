using Service.Services;
using Service.Services.AppAccount;
using Service.Services.Auction;
using Service.Services.RealEstate;
using Service.Services.VnpayService.VnpayUtility;

namespace RazorAucionWebapp.Configure
{
	public static class RegisterServices
	{
		public static IServiceCollection AddMyServices(this IServiceCollection services) 
		{

			services.AddScoped<VnpayBuildUrl>();
			services.AddScoped<VnpayQuery>();
			services.AddScoped<VnpayRefund>();
			services.AddScoped<VnpayReturn>();
			services.AddScoped<VnpayAvailableServices>();

			services.AddScoped<BidServices>();

			services.AddScoped<ImageService>();
			services.AddScoped<EstateImagesServices>();
			services.AddScoped<AccountImagesServices>();

			services.AddScoped<EstateServices>();
			services.AddScoped<EstateCategoryDetailServices>();
			services.AddScoped<EstateCategoriesServices>();

            services.AddScoped<JoinedAuctionServices>();
            services.AddScoped<AuctionServices>();
			services.AddScoped<AuctionReceiptServices>();	


			services.AddScoped<CompanyServices>();
			services.AddScoped<AccountServices>();

			

			return services;
		}
	}
}
