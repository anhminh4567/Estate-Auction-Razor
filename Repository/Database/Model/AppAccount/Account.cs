using Repository.Database.Model.AuctionRelated;
using Repository.Database.Model.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Database.Model.AppAccount
{
    public class Account
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AccountId { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string FullName { get; set; }
        public DateTime Dob { get; set; }
        public string Telephone { get; set; }
        public AccountStatus Status { get; set; }
        public Role Role { get; set; }
        public string CMND { get; set; }
        public decimal Balance { get; set; }
        public IList<Transaction> Transactions { get; set; }
        public IList<AuctionReceipt> AuctionsReceipt { get; set; }
        public IList<Bid> Bids { get; set; }
        public IList<AccountImages> AccountImages { get; set; }
		public IList<JoinedAuction>? JoinedAuctions { get; set; }
        public IList<AuctionReceiptPayment>? ReceiptPayments { get; set; }
        public IList<Notification>? Notifications { get; set; }
	}
}
