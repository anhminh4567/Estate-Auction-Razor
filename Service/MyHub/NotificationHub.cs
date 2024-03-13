using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.MyHub
{
    public class NotificationHub : Hub
    {
        private readonly ConnectionMapping _connectionMapping;
        public NotificationHub(ConnectionMapping connectionMapping) 
        {
            _connectionMapping = connectionMapping;
        }

        public override async Task OnConnectedAsync()
        {
            var context = Context.GetHttpContext();
            _connectionMapping.Add(context.User.Identity.Name, Context.ConnectionId);
            await Groups.AddToGroupAsync(connectionId: Context.ConnectionId, "notifications");
        }
        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var context = Context.GetHttpContext();
            _connectionMapping.Remove(context.User.Identity.Name);
            await Groups.RemoveFromGroupAsync(connectionId: Context.ConnectionId, "notifications");
        }

    }
}
