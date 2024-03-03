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
	public class AuctionReceiptPayment
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }
		public int? AccountId { get; set; }
		public Account? Account { get; set; }
		public int? ReceiptId { get; set; }
		public AuctionReceipt? Receipt { get; set; }
		public decimal PayAmount { get; set; }
		public DateTime PayTime { get; set; }
	}
}
