using DAL;
using DTO;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using UI.Controllers;

namespace UI
{
    [HubName("chatHub")]
    public class ChatHub : Hub
    {
        HealthHelperEntities dbContext = new HealthHelperEntities();

        public override Task OnConnected()
        {
            
            UserHandler.ConnectedIds.Add(Context.ConnectionId);
            UserDetail user = new UserDetail
            {
                ConnID = Context.ConnectionId,
                UserID = Context.User.Identity.Name,
            };
            UserStatic.ConnectedUsers.Add(user);

            //==================================================
            //Reconnect When Return
            foreach (var groupId in UserStatic.UserChatGroups.Keys.ToList())
            {
                foreach (var member in UserStatic.UserChatGroups[groupId].GroupMembers.ToList())
                {
                    if (member.UserID == Context.User.Identity.Name)
                    {
                        UserStatic.UserChatGroups[groupId].GroupMembers.Add(new UserDetail
                        {
                            ConnID = Context.ConnectionId,
                            UserID = Context.User.Identity.Name
                        });

                        Groups.Add(Context.ConnectionId, groupId);
                    }
                }
            }

            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled = true)
        {
            UserDetail disconntectedUser = UserStatic.ConnectedUsers
                .FirstOrDefault(x => x.ConnID == Context.ConnectionId);
            UserStatic.ConnectedUsers.Remove(disconntectedUser);
            UserHandler.ConnectedIds.Remove(Context.ConnectionId);

            //========================================================
            //恩旗
            //Remove User From UserChatGroups
            try
            {
                foreach (var groupId in UserStatic.UserChatGroups.Keys.ToList())
                {
                    foreach (var member in UserStatic.UserChatGroups[groupId].GroupMembers.ToList())
                    {
                        if (member.ConnID == Context.ConnectionId)
                        {
                            UserStatic.UserChatGroups[groupId].GroupMembers.Remove(member);
                        }

                        ChatGroupController.RemoveGroup(this.dbContext, groupId);
                    }
                }

            }
            catch (Exception ex)
            {
                string filePath = @"C:\Users\enchi\Desktop\Error.txt";

                using (StreamWriter writer = new StreamWriter(filePath, true))
                {
                    writer.WriteLine(DateTime.Now.ToString("M/d HH:mm") + " Message : " + ex.Message);
                }
            }
            
            //========================================================

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