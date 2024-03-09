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
        public int CompanyId { get; set; }
        public Company? Company { get; set; }
        public int AccountId { get; set; }
        public Account? Account { get; set; }
        public NotificationType Type { get; set; }
        public string Message { get; set; }
        public bool IsChecked { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
