using Repository.Database.Model;
using Repository.Database.Model.AppAccount;
using Repository.Interfaces.DbTransaction;


namespace Service.Services
{
	public class NotificationServices
	{
		private readonly IUnitOfWork _unitOfWork;
		public NotificationServices(IUnitOfWork unitOfWork) 
		{
			_unitOfWork = unitOfWork;
		}

		public async Task<List<Notification>?> GetAllNotification(int id)
		{
			return await _unitOfWork.Repositories.notificationRepository.GetByCondition(p => p.CompanyId == id, p => p.OrderBy(p => p.CreatedDate));
		}

		public async Task<Notification?> GetNotificationById(int id)
		{
			return (await _unitOfWork.Repositories.notificationRepository.GetByCondition(p => p.NotificationId == id)).FirstOrDefault();
		}

        public async Task CreateNotification(Notification notification)
		{

			await _unitOfWork.Repositories.notificationRepository.CreateAsync(notification);
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

    }
}
