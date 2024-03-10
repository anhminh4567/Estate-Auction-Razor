using Microsoft.AspNetCore.SignalR;

namespace Service.MyHub
{
	public class BidHub : Hub
	{
		public BidHub()
		{
		}

		public override async Task OnConnectedAsync()
		{
			await Groups.AddToGroupAsync(Context.ConnectionId, "all"); 
		}

		public override async Task OnDisconnectedAsync(Exception? exception)
		{
			await Groups.RemoveFromGroupAsync(Context.ConnectionId, "all");
		}
	}
}
