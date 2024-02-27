using Repository.Database.Model.AuctionRelated;
using Repository.Database.Model.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Database.Model.AppAccount
{
    public class JoinedAuction
    {
        public int? AccountId { get; set; }
        public Account? Account { get; set; }
        public int? AuctionId { get; set; }
        public Auction? Auction { get; set; }
        public int ? TransactionId { get; set; }
        public Transaction? Transaction { get; set; }
        public JoinedAuctionStatus Status { get; set; }
        public DateTime RegisterDate { get; set; }
        public decimal RegiisterFee { get; set; }

    }
}
