using Repository.Database.Model;
using Repository.Database.Model.Enum;
using Repository.Interfaces.DbTransaction;
using Service.MyHub.HubServices;

namespace Service.Services
{
	public class NotificationServices
	{
		private readonly IUnitOfWork _unitOfWork;
        private readonly NotificationHubService _notificationHubService;
		public NotificationServices(IUnitOfWork unitOfWork, NotificationHubService notificationHubService) 
		{
			_unitOfWork = unitOfWork;
            _notificationHubService = notificationHubService;

        }

		public async Task<List<Notification>?> GetAllNotification(int id)
		{
            return await _unitOfWork.Repositories.notificationRepository.GetByCondition(p => p.ReceiverId == id, p => p.OrderByDescending(p => p.NotificationId), includeProperties: "Sender,Receiver");
		}
        public async Task<List<Notification>?> GetUncheckedMail(int id)
        {
			return await _unitOfWork.Repositories.notificationRepository.GetByCondition(p => p.ReceiverId == id && !p.IsChecked );
        }
        public async Task<Notification?> GetNotificationById(int id)
		{
			return (await _unitOfWork.Repositories.notificationRepository.GetByCondition(p => p.NotificationId == id)).FirstOrDefault();
		}

        public async Task CreateNotification(Notification notification)
		{

		}

		public async Task SetToChecked(int id)
		{
            var notification = await GetNotificationById(id);
			if (notification is not null)
			{
                notification.IsChecked = true;
                await _unitOfWork.Repositories.notificationRepository.UpdateAsync(notification);
            }
		}

		public async Task<bool> CheckUnreadMail(int id)
		{
			var list = await _unitOfWork.Repositories.notificationRepository.GetByCondition(p => p.ReceiverId == id && !p.IsChecked );
			return list.Count != 0;
		}

        public async Task<Notification> CreateMessage(int senderId, int receiverId, int auctionId, NotificationType type)
        {
            var sender = (await _unitOfWork.Repositories.accountRepository.GetByCondition(p => p.AccountId == senderId)).FirstOrDefault();
            var estate = (await _unitOfWork.Repositories.auctionRepository.GetByCondition(p => p.AuctionId == auctionId, includeProperties: "Estate")).FirstOrDefault().Estate;
            var message = "";
            switch (type)
            {
                case NotificationType.JoinAuction:
                    message = String.Format("{0} joined your auction({1})", sender.FullName, estate.Name);
                    break;
                case NotificationType.QuitAuction:
                    message = String.Format("{0} quited your auction({1})", sender.FullName, estate.Name);
                    break;
                case NotificationType.UpdateAuction:
                    message = String.Format("{0} updated an auction({1}) that you have joined", sender.FullName, estate.Name);
                    break;
                case NotificationType.CancelAuction:
                    message = String.Format("{0} cancelled an auction({1}) that you have joined", sender.FullName, estate.Name);
                    break;
                case NotificationType.StartAuction:
                    message = String.Format("an auction({0}) that you have joined has started", sender.FullName, estate.Name);
                    break;
                case NotificationType.EndAuction:
                    message = String.Format("an auction({0}) that you have joined has ended", sender.FullName, estate.Name);
                    break;
                case NotificationType.AdminCancelAuction:
                    message = String.Format("Admin has cancelled one of your auction({1}) ", estate.Name);
                    break;
            }

            return new Notification()
            {
                SenderId = senderId,
                ReceiverId = receiverId,
                Type = type,
                Message = message,
                IsChecked = false,
                CreatedDate = DateTime.Now.Date
            };
        }
        //for user join and quit
        public async Task SendMessage(int senderId, int auctionId, NotificationType type)
        {
            int receiverId = (await _unitOfWork.Repositories.auctionRepository.GetByCondition(p => p.AuctionId == auctionId, includeProperties: "Estate.Company")).FirstOrDefault().Estate.Company.AccountId;
            var receiver = (await _unitOfWork.Repositories.accountRepository.GetByCondition(p => p.AccountId == receiverId)).FirstOrDefault();
            var notification = await CreateMessage(senderId, receiverId, auctionId, type);
			await _unitOfWork.Repositories.notificationRepository.CreateAsync(notification);
            await _notificationHubService.SendNewNotification(receiver.Email);
        }
        //For update, cancel, start and end
        public async Task SendMessage(int auctionId, NotificationType type)
        {
            var auction = (await _unitOfWork.Repositories.auctionRepository.GetByCondition(p => p.AuctionId == auctionId, includeProperties: "Account, Estate.Company")).FirstOrDefault();
            if (type == NotificationType.AdminCancelAuction)
            {
                int senderId = 0;
                int receiverId = auction.Estate.Company.AccountId;
                var notification = await CreateMessage(senderId, receiverId, auctionId, type);
			    await _unitOfWork.Repositories.notificationRepository.CreateAsync(notification);
                await _notificationHubService.SendNewNotification(auction.Estate.Company.Email);
            }
            else
            {
                var joinedAucitons = await _unitOfWork.Repositories.joinedAuctionRepository.GetByCondition(p => p.AuctionId == auctionId, includeProperties: "Account, Auction.Estate");
                foreach (var joinedAuction in joinedAucitons)
                {
                    int senderId = joinedAuction.AccountId.Value;
                    int receiverId = joinedAuction.Auction.Estate.CompanyId;
                    var notification = await CreateMessage(senderId, receiverId, auctionId, type);
			        await _unitOfWork.Repositories.notificationRepository.CreateAsync(notification);
                    await _notificationHubService.SendNewNotification(joinedAuction.Account.Email);
                }
            }
        }
    }
}
