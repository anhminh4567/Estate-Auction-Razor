using Repository.Database.Model.AppAccount;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Database.Model.AuctionRelated
{
    public class Bid
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BidId { get; set; }
        public int AuctionId { get; set; }
        public Auction Auction { get; set; }
        public int BidderId { get; set; }
        public Account Bidder { get; set; }
        public decimal Amount { get; set; }
        public DateTime Time { get; set; }
    }
}
