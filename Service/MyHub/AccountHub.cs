using Microsoft.AspNetCore.SignalR;

namespace Service.MyHub
{
	public class AccountHub : Hub
	{
		public AccountHub()
		{
		}

		public override Task OnConnectedAsync()
		{
			return base.OnConnectedAsync();
		}

		public override Task OnDisconnectedAsync(Exception? exception)
		{
			return base.OnDisconnectedAsync(exception);
		}
	}
}
