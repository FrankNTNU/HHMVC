﻿using BLL;
using DAL;
using DTO;
using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace UI.Areas.Admin.Controllers
{
    public class ContactController : BaseController
    {
        HealthHelperEntities dbContext = new HealthHelperEntities();

        // GET: Admin/Contact
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Contact()
        {
            return View();
        }
        public int GetCounts()
        {
            return UserStatic.ConnectedUsers.Count();
        }
        UserBLL userBLL = new UserBLL();
        public JsonResult GetOnlineUsers()
        {
            foreach (var item in UserStatic.ConnectedUsers)
            {
                UserDTO dto = userBLL.GetUserWithID(Int32.Parse(item.UserID));
                item.UserName = dto.UserName;
                item.ImagePath = dto.ImagePath;
            }
            return Json(UserStatic.ConnectedUsers, JsonRequestBehavior.AllowGet);
            
        }

        public JsonResult HasChanged(string previousList)
        {
            string newList = "";
            foreach (var item in UserHandler.ConnectedIds)
            {
                newList += item;
            }
            if (newList != previousList)
            {
                return Json("true");
            }
            else
            {
                return Json("false");
            }
        }

        [HttpPost]
        public ActionResult Contact(string connId, string message)
        {
            var context = GlobalHost.ConnectionManager.GetHubContext<ChatHub>();

            context.Clients.Client(connId).ReceiveFromService(message);

            return View();
        }

        //=============================================================
        //恩旗
        [HttpPost]
        public JsonResult GetServiceGroups(string connId)
        {
            //====================================================================

            var userList = UserStatic.ServiceGroups.Where(sg => sg.Value.AdminConnId == connId)
                .Select(sg =>
            {
                int UserID = int.Parse(sg.Value.GroupName);
                Member member = dbContext.Members.SingleOrDefault(m => m.ID == UserID);
                return new
                {
                    ImagePath = member.Image,
                    GroupID = sg.Key,
                    UserName = member.UserName
                };
            }).ToList();

            return Json(userList);
        }

        [HttpPost]
        public JsonResult AddToServiceGroups(string connId)
        {
            //If already Reconnect to old Group
            foreach (var groupId in UserStatic.ServiceGroups.Keys.ToList())
            {
                if (UserStatic.ServiceGroups[groupId].AdminId == User.Identity.Name)
                {
                    return Json(new { Result = "Reconnect to old group" });
                }
            }

            int UserId = int.Parse(User.Identity.Name);
            Member member = dbContext.Members.SingleOrDefault(m => m.ID == UserId);

            if (member.IsAdmin)
            {
                var Context = GlobalHost.ConnectionManager.GetHubContext<ChatHub>();

                foreach (var groupId in UserStatic.ServiceGroups.Keys.ToList())
                {
                    if (UserStatic.ServiceGroups[groupId].AdminConnId == "")
                    {
                        UserStatic.ServiceGroups[groupId].AdminConnId = connId;
                        UserStatic.ServiceGroups[groupId].AdminId = User.Identity.Name;

                        Context.Groups.Add(connId, groupId);
                    }
                }
                return Json(new { Result = "Success" });
            }
            else
            {
                return Json(new { Result = "Not an admin" });
            }
        }

        [HttpPost]
        public void SendMessage(string connId, string groupId, string message)
        {

            var Context = GlobalHost.ConnectionManager.GetHubContext<ChatHub>();

            DateTime timeStamp = DateTime.Now;

            Context.Clients.Group(groupId)
                .receive(connId, "客服人員", message, timeStamp.ToString("M/d HH:mm"), "e9ec5c93-c442-4d6d-96d1-fc2fb8c570fcuser2.png", groupId);

            
            dbContext.GroupChats.Add(new GroupChat
            {
                GroupID = int.Parse(groupId),
                MemberID = int.Parse(User.Identity.Name),
                Message = message,
                TimeStamp = timeStamp
            });

            dbContext.SaveChanges();
        }

        public JsonResult GetGroupMsgs(string groupId)
        {
            var msgList = dbContext.GroupChats.Where(gc => gc.GroupID.ToString() == groupId)
                .OrderBy(gc => gc.TimeStamp).ToList()
                .Select(gc =>
                {
                    string connId = "";
                    string userName = "";
                    string image = "";

                    if (gc.MemberID == null)
                    {
                        return new
                        {
                            connId = UserStatic.ServiceGroups[groupId].AdminConnId,
                            userName = "客服人員",
                            message = gc.Message,
                            timeStamp = gc.TimeStamp.ToString("M/d HH:mm"),
                            image = "e9ec5c93-c442-4d6d-96d1-fc2fb8c570fcuser2.png"
                        };
                    }
                    else
                    {
                        Member member = dbContext.Members.SingleOrDefault(m => m.ID == gc.MemberID);

                        if (member.IsAdmin)
                        {
                            connId = UserStatic.ServiceGroups[groupId].AdminConnId;
                            userName = "客服人員";
                            image = "e9ec5c93-c442-4d6d-96d1-fc2fb8c570fcuser2.png";
                        }
                        else
                        {
                            connId = UserStatic.ServiceGroups[groupId].UserConnId;
                            userName = member.UserName;
                            image = member.Image;
                        }

                        return new
                        {
                            connId = connId,
                            userName = userName,
                            message = gc.Message,
                            timeStamp = gc.TimeStamp.ToString("M/d HH:mm"),
                            image = image
                        };
                    }
                });

            return Json(msgList);
        }
    }
}