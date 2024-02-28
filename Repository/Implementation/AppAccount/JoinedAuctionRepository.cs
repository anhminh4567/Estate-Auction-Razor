using Repository.Database;
using Repository.Database.Model.AppAccount;
using Repository.Database.Model.Enum;
using Repository.Interfaces;
using Repository.Interfaces.AppAccount;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Implementation.AppAccount
{
    public class JoinedAuctionRepository : BaseRepository<JoinedAuction>, IJoinedAuctionRepository
    {
        public JoinedAuctionRepository(AuctionRealEstateDbContext context) : base(context)
        {

        }

        public async Task<bool> IsJoined(int accountId, int auctionId)
        {
            var item = await _set.FindAsync(new {accountId = accountId, auctionId = auctionId, Status = JoinedAuctionStatus.REGISTERED });
            return item == null;
        }
    }
}
