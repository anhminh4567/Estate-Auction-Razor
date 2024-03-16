using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repository.Database.Model.AppAccount;
using Repository.Database.Model.AuctionRelated;

namespace Service.Interfaces.HubServices
{
    public interface IBidHubServices
    {
        Task SendNewBid(Bid newBid, Account bidder);
        Task DeleteBids(List<Bid> deletedBids);

    }
}
