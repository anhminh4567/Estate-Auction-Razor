using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repository.Database.Model;
using Repository.Database.Model.Enum;

namespace Service.Interfaces
{
    public interface INotificationServices
    {
        Task<List<Notification>?> GetAllNotification(int id);
        Task<List<Notification>?> GetUncheckedMail(int id);
        Task<Notification?> GetNotificationById(int id);
        Task CreateNotification(Notification notification);
        Task SetToChecked(int id);
        Task<bool> CheckUnreadMail(int id);
        Task<Notification> CreateMessage(int senderId, int receiverId, int auctionId, NotificationType type);
        Task SendMessage(int senderId, int auctionId, NotificationType type);
        Task SendMessage(int auctionId, NotificationType type);
    }
}
