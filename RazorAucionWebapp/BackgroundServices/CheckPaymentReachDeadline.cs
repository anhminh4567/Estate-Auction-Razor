using Repository.Database.Model.AppAccount;
using Repository.Database.Model.AuctionRelated;
using Repository.Database.Model.Enum;
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
        public override Task StartAsync(CancellationToken cancellationToken)
        {
            return base.StartAsync(cancellationToken);
        }
        public override Task StopAsync(CancellationToken cancellationToken)
        {
            return base.StopAsync(cancellationToken);
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                //await CheckIfAuctionStartOrEnd();
                Console.WriteLine("checking if payment reach deadline");
                await Task.Delay(30000, stoppingToken); // Wait for 30 seconds
            }
        }
        private async Task CheckIfPaymentReachTheEnd() 
        {
            using (var scope = ServiceProvider.CreateScope())
            {
                var auctionService = scope.ServiceProvider.GetRequiredService<AuctionServices>();
                var auctionRecieptService = scope.ServiceProvider.GetRequiredService<AuctionReceiptServices>();
                var getAllAuctionReceipt = await auctionRecieptService.GetWithCondition(includeProperties: "Auction,Buyer");
                foreach(var Receipt in getAllAuctionReceipt)
                {
                    var auction = Receipt.Auction;
                    var buyer = Receipt.Buyer;
                    var endPayDate = auction.EndPayDate;
                    var remainAmount = Receipt.RemainAmount;
                    if(DateTime.Compare(DateTime.Now,endPayDate) >= 0)
                    {
                        if(remainAmount > 0) // the user haven't paid off the debt
                        {
                            auction.Status = AuctionStatus.FAILED_TO_PAY;
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
