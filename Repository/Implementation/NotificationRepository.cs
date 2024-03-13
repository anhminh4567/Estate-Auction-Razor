using Repository.Database;
using Repository.Database.Model;
using Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Implementation
{
	public class NotificationRepository : BaseRepository<Notification>, INotificationRepository
	{
		public NotificationRepository(AuctionRealEstateDbContext context) : base(context)
		{

		}
	}
}
