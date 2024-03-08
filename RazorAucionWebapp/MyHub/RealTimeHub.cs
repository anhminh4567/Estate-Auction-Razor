using Microsoft.AspNetCore.SignalR;
using Repository.Database.Model.AppAccount;

namespace RazorAucionWebapp.MyHub
{
    public class RealTimeHub : Hub
    {
        public RealTimeHub()
        {
        }

        public override async Task OnConnectedAsync()
        {
            var context = Context.GetHttpContext();
            // use identity to set to group if necesesary
            await Clients.Caller.SendAsync("ReceiveMessage",Context.ConnectionId.ToString());
        }
        public override Task OnDisconnectedAsync(Exception? exception)
        {
            return base.OnDisconnectedAsync(exception);
        }
        //public async Task SendObject() {
        //    var objectToSend = new Company(true);
        //    await Clients.Caller.SendAsync("RecieveObject", objectToSend);
        //}
        internal async Task CreateObject(Type type, object objectSend)
        {
            await Clients.All.SendAsync("CreatedObject", type, objectSend);
        }
        internal async Task DeleteObject(Type type, object objectSend)
        {
            await Clients.All.SendAsync("DeletedObject", type, objectSend);
        }
        internal async Task UpdateObject(Type type, object objectSend)
        {
            await Clients.All.SendAsync("UpdatedObject", type, objectSend);
        }
    }
}
