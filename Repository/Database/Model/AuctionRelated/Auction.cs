using Repository.Database.Model.AppAccount;
using Repository.Database.Model.Enum;
using Repository.Database.Model.RealEstate;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Database.Model.AuctionRelated
{
    public class Auction
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AuctionId { get; set; }
        public int EstateId { get; set; }
        public Estate Estate { get; set; }
        public DateTime RegistrationDate { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal EntranceFee { get; set; }
        public decimal WantedPrice { get; set; }
        public decimal IncrementPrice { get; set; }
        public int MaxParticipant { get; set; }
        public AuctionStatus Status { get; set; }
        public IList<Bid> Bids { get; set; }
        public AuctionReceipt? AuctionReceipt { get; set; }
        public IList<JoinedAuction>? JoinedAccounts { get; set;}
    }
}
