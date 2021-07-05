using DTO;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace UI
{
    [HubName("chatHub")]
    public class ChatHub : Hub
    {
        public override Task OnConnected()
        {
            UserHandler.ConnectedIds.Add(Context.ConnectionId);
            UserDetail user = new UserDetail
            {
                ConnID = Context.ConnectionId,
                UserID = Context.User.Identity.Name,
            };
            UserStatic.ConnectedUsers.Add(user);
            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled = true)
        {
            UserDetail disconntectedUser = UserStatic.ConnectedUsers
                .FirstOrDefault(x => x.ConnID == Context.ConnectionId);
            UserStatic.ConnectedUsers.Remove(disconntectedUser);
            UserHandler.ConnectedIds.Remove(Context.ConnectionId);
            return base.OnDisconnected(stopCalled);
        }
        public void Send(string name, string imagePath, string message)
        {
            // Call the addNewMessageToPage method to update clients.
            Clients.All.addNewMessageToPage(name, imagePath, message);
            //Clients.Client(connID).addNewMessageToPage(name, imagePath, message);
        }

    }
}