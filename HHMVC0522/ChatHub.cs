﻿using DAL;
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

            //=====================================================
            //For CustomerService

            int UserId = int.Parse(Context.User.Identity.Name);
            Member member = dbContext.Members.SingleOrDefault(m => m.ID == UserId);

            //Admin Reconnect When Return
            if (member.IsAdmin)
            {
                foreach (var groupId in UserStatic.ServiceGroups.Keys.ToList())
                {
                    if (UserStatic.ServiceGroups[groupId].AdminId == Context.User.Identity.Name
                        && UserStatic.ServiceGroups[groupId].AdminConnId != Context.ConnectionId)
                    {
                        Groups.Remove(UserStatic.ServiceGroups[groupId].AdminConnId, groupId);

                        UserStatic.ServiceGroups[groupId].AdminConnId = Context.ConnectionId;

                        Groups.Add(Context.ConnectionId, groupId);
                    }
                }
            }
            //User Reconnect When Return
            else
            {
                foreach (var groupId in UserStatic.ServiceGroups.Keys.ToList())
                {
                    if (UserStatic.ServiceGroups[groupId].GroupName == Context.User.Identity.Name
                        && UserStatic.ServiceGroups[groupId].UserConnId != Context.ConnectionId)
                    {
                        Groups.Remove(UserStatic.ServiceGroups[groupId].UserConnId, groupId);

                        UserStatic.ServiceGroups[groupId].UserConnId = Context.ConnectionId;

                        Groups.Add(Context.ConnectionId, groupId);
                    }
                }
            }
            
            //=====================================================

            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled = true)
        {
            UserDetail disconntectedUser = UserStatic.ConnectedUsers
                .FirstOrDefault(x => x.ConnID == Context.ConnectionId);
            UserStatic.ConnectedUsers.Remove(disconntectedUser);
            UserHandler.ConnectedIds.Remove(Context.ConnectionId);

            //For CustomerService
            Member member = dbContext.Members
                .SingleOrDefault(m => m.ID.ToString() == Context.User.Identity.Name);

            try
            {   //When Admin disconnect, only remove AdminConnId and AdminId
                if (member.IsAdmin)
                {
                    foreach (var groupId in UserStatic.ServiceGroups.Keys.ToList())
                    {
                        if (UserStatic.ServiceGroups[groupId].AdminConnId == Context.ConnectionId)
                        {
                            Groups.Remove(UserStatic.ServiceGroups[groupId].AdminConnId, groupId);

                            UserStatic.ServiceGroups[groupId].AdminConnId = "";
                            UserStatic.ServiceGroups[groupId].AdminId = "0";

                            dbContext.SaveChanges();
                        }
                    }
                }
                //When User disconnect, remove ServiceGroup
                else
                {
                    foreach (var groupId in UserStatic.ServiceGroups.Keys.ToList())
                    {
                        if (UserStatic.ServiceGroups[groupId].UserConnId == Context.ConnectionId)
                        {
                            Groups.Remove(UserStatic.ServiceGroups[groupId].UserConnId, groupId);

                            if (UserStatic.ServiceGroups[groupId].AdminConnId != "")
                            {
                                Groups.Remove(UserStatic.ServiceGroups[groupId].AdminConnId, groupId);
                            }
                            
                            UserStatic.ServiceGroups.Remove(groupId);

                            dbContext.Groups.SingleOrDefault(g => g.ID.ToString() == groupId).IsAlive = false;

                            dbContext.SaveChanges();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string filePath = @"C:\Users\enchi\Desktop\Error2.txt";

                using (StreamWriter writer = new StreamWriter(filePath, true))
                {
                    writer.WriteLine(DateTime.Now.ToString("M/d HH:mm") + " Message : " + ex.Message);
                }
            }

            return base.OnDisconnected(stopCalled);
        }

    }
}