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
    [HubName("groupChatHub")]
    public class GroupChatHub : Hub
    {
        HealthHelperEntities dbContext = new HealthHelperEntities();

        public override Task OnConnected()
        {
            
            //UserHandler.ConnectedIds.Add(Context.ConnectionId);

            UserDetail user = null;
            bool isAdded = false;

            foreach (var User in UserStatic.ChatGroupUsers)
            {
                if (User.ConnID == Context.ConnectionId && User.UserID == Context.User.Identity.Name)
                {
                    isAdded = true;
                    user = User;
                    user.Role = "customer";
                    break;
                }
            }

            if (!isAdded)
            {
                user = new UserDetail
                {
                    ConnID = Context.ConnectionId,
                    UserID = Context.User.Identity.Name,
                    Role = "customer"
                };
                UserStatic.ChatGroupUsers.Add(user);
            }
            
            //==================================================
            //For GroupChats
            //Reconnect When Return
            foreach (var groupId in UserStatic.UserChatGroups.Keys.ToList())
            {
                foreach (var chatMember in UserStatic.UserChatGroups[groupId].GroupMembers.ToList())
                {
                    if (chatMember.UserID == Context.User.Identity.Name
                        && chatMember.ConnID != Context.ConnectionId)
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
            UserDetail disconntectedUser = UserStatic.ChatGroupUsers
                .SingleOrDefault(x => x.ConnID == Context.ConnectionId);
            UserStatic.ChatGroupUsers.Remove(disconntectedUser);
            //UserHandler.ConnectedIds.Remove(Context.ConnectionId);

            //========================================================
            //恩旗
            //For GroupChats
            //Remove User From UserChatGroups

            foreach (var groupId in UserStatic.UserChatGroups.Keys.ToList())
            {
                foreach (var chatMember in UserStatic.UserChatGroups[groupId].GroupMembers.ToList())
                {
                    if (chatMember.ConnID == Context.ConnectionId)
                    {
                        Groups.Remove(Context.ConnectionId, groupId);
                        UserStatic.UserChatGroups[groupId].GroupMembers.Remove(chatMember);
                    }

                    ChatGroupController.RemoveGroup(this.dbContext, groupId);
                }
            }
            
            return base.OnDisconnected(stopCalled);
        }

    }
}