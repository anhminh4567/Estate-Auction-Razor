using Repository.Database.Model.AppAccount;
using Repository.Database.Model.Enum;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Repository.Database.Model
{
    public class Notification
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int NotificationId { get; set; }
        public int ReceiverId { get; set; }
        public Account? Receiver { get; set; }
        public int SenderId { get; set; }
        public Account? Sender { get; set; }
        public NotificationType Type { get; set; }
        public string Message { get; set; }
        public bool IsChecked { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
