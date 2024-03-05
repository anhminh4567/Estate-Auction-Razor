using RazorAucionWebapp.Configure;
using Repository.Database;
using Repository.Database.Model.AuctionRelated;
using Repository.Database.Model.Enum;
using Service.Services.Auction;

namespace RazorAucionWebapp.BackgroundServices
{
    public class CheckAuctionTimeWorker : BackgroundService
    {
        public IServiceProvider ServiceProvider { get; }
        public CheckAuctionTimeWorker(IServiceProvider serviceProvider)
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
                await CheckIfAuctionStartOrEnd();
                Console.WriteLine("checking auction is running");
                await Task.Delay(30000, stoppingToken); // Wait for 30 seconds
            }
        }
        private async Task CheckIfAuctionStartOrEnd() 
        {
            using (var scope = ServiceProvider.CreateScope()) 
            {
                var getBindAppsettings = scope.ServiceProvider.GetRequiredService<BindAppsettings>();
                var auctionService = scope.ServiceProvider.GetRequiredService<AuctionServices>();
				var bidServices = scope.ServiceProvider.GetRequiredService<BidServices>();
                var auctionRecieptService = scope.ServiceProvider.GetRequiredService<AuctionReceiptServices>();
				var getAll = await auctionService.GetWithCondition(a =>
                a.Status.Equals(AuctionStatus.NOT_STARTED) ||
                a.Status.Equals(AuctionStatus.ONGOING));
                foreach ( var auc in getAll ) 
                { 
                    if(auc.Status.Equals(AuctionStatus.NOT_STARTED) && auc.StartDate.CompareTo(DateTime.Now) <= 0) 
                    {
                        auc.Status = AuctionStatus.ONGOING; // chane from not started to ready 
                    }
                    if(auc.Status.Equals(AuctionStatus.ONGOING) && auc.EndDate.CompareTo(DateTime.Now) <= 0) 
                    {
                        var getFullDetail = await auctionService.GetInclude(auc.AuctionId, "JoinedAccounts,Bids,Estate");
                        if(getFullDetail.Bids is null ||
                            getFullDetail.JoinedAccounts is null ||
                            getFullDetail.Bids.Count() == 0 ||
                            getFullDetail.JoinedAccounts.Count == 0 ) 
                        {
                            auc.Status = AuctionStatus.CANCELLED;
                        }
                        else 
                        {
                            auc.Status = AuctionStatus.PENDING_PAYMENT; //waiting for payment
                            var getHighestBid = await bidServices.GetHighestBids(auctionId: auc.AuctionId);
                            if (getHighestBid is null)
                                auc.Status = AuctionStatus.CANCELLED;// cancelled if no bid is placed
                            else
                            {
                                var newWinnder = new AuctionReceipt()
                                {
                                    AuctionId = auc.AuctionId,
                                    BuyerId = getHighestBid.BidderId,
                                    Amount = getHighestBid.Amount,
                                    RemainAmount = getHighestBid.Amount - 0,
                                    Commission = 0, // this is because commision only apllied when the user has already paid all 
                                };
                                var createResult = await auctionRecieptService.Create(newWinnder);
                                if (createResult is null)
                                    throw new Exception("something wrong when creating new reciept, in backgroundService");
                               auc.ReceiptId = createResult.ReceiptId;
                                // auc.Estate.Status = EstateStatus.FINISHED;
                            }
                        }
                    }
                    var tryUpdte = await auctionService.Update( auc );  
                }
            }
        }
    }
}
