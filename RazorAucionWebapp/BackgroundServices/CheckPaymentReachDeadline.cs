using RazorAucionWebapp.Configure;
using Repository.Database.Model.AppAccount;
using Repository.Database.Model.AuctionRelated;
using Repository.Database.Model.Enum;
using Service.Services.AppAccount;
using Service.Services.Auction;

namespace RazorAucionWebapp.BackgroundServices
{
	public class CheckPaymentReachDeadline : BackgroundService
	{
		public IServiceProvider ServiceProvider { get; }

		public CheckPaymentReachDeadline(IServiceProvider serviceProvider)
		{
			ServiceProvider = serviceProvider;
		}
		protected override async Task ExecuteAsync(CancellationToken stoppingToken)
		{
			while (!stoppingToken.IsCancellationRequested)
			{
				await CheckIfPaymentReachTheEnd();
				Console.WriteLine("checking if payment reach deadline");
				await Task.Delay(12000, stoppingToken); // Wait for 30 seconds
			}
		}
		private async Task CheckIfPaymentReachTheEnd()
		{
			using (var scope = ServiceProvider.CreateScope())
			{
				var auctionService = scope.ServiceProvider.GetRequiredService<AuctionServices>();
				var auctionRecieptService = scope.ServiceProvider.GetRequiredService<AuctionReceiptServices>();
				var auctionRecieptPaymentService = scope.ServiceProvider.GetRequiredService<AuctionReceiptPaymentServices>();
				var accountService = scope.ServiceProvider.GetRequiredService<AccountServices>();
				var getAllAuctionReceipt = await auctionRecieptService.GetWithCondition(includeProperties: "Auction,Buyer,Payments");
				var getBindAppsetting = scope.ServiceProvider.GetRequiredService<BindAppsettings>();
				foreach (var Receipt in getAllAuctionReceipt)
				{
					var auction = Receipt.Auction;
					var buyer = Receipt.Buyer;
					var payments = Receipt.Payments;
					var endPayDate = auction.EndPayDate;
					var remainAmount = Receipt.RemainAmount;
					if (auction.Status.Equals(AuctionStatus.SUCCESS) ||
					   auction.Status.Equals(AuctionStatus.FAILED_TO_PAY))
					{
						continue;
					}
					if (DateTime.Compare(DateTime.Now, endPayDate) >= 0)
					{
						if (remainAmount > 0) // the user haven't paid off the debt
						{
							auction.Status = AuctionStatus.FAILED_TO_PAY;
							var amountUserHavePaid = Receipt.Amount - Receipt.RemainAmount;
							//entrence fee ko trar laij cho thang nay do vi pham luat
							// Compnay balance cung ko dc cong, do ngay khi thang bid, balance cua Company da dc cong
							buyer.Balance += amountUserHavePaid - auction.EntranceFee;
							// remove all Payment if the account is not paid on time
							if (payments.Count > 0)
							{
								await auctionRecieptPaymentService.DeleteRange(payments.ToList());
							}
							//Receipt.Commission = getBindAppsetting.ComissionFixedPrice; // van lay tien commission cho app
							Receipt.RemainAmount = Receipt.Amount - auction.EntranceFee;// remain se la tien tong tru di entrence fee, do company van an entrence fee
							await auctionRecieptService.Update(Receipt);
							await accountService.Update(buyer);
							await DoSomethingIfFailed(Receipt);
						}
					}
				}
			}
		}
		private Task DoSomethingIfFailed(AuctionReceipt currentReceipt)
		{
			// send mail or notify admin, company, i dontk now            
			return Task.CompletedTask;
		}
	}
}
