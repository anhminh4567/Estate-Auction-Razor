using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repository.Database.Model.AppAccount;
using Repository.Database.Model.Enum;

namespace Repository.Database.Model
{
    public class Transaction
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TransactionId { get; set; }
        public int? AccountId { get; set; }
        public Account? Account { get; set; }
        public TransactionStatus Status { get; set; }
        public string vnp_TxnRef { get; set; }
        public string vnp_Amount { get; set; }
        public long vnp_TransactionDate { get; set; }
        public string vnp_OrderInfo { get; set; }
        public string vnp_PayDate { get; set; }
    }
}
