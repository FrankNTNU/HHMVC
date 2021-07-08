using BLL;
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
        [System.Web.Mvc.Authorize]
        public int GetCounts()
        {
            
            return UserStatic.ConnectedUsers.Count();
        }

        readonly UserBLL userBLL = new UserBLL();
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

            var userList = UserStatic.ServiceGroups.Where(sg => 
            {
                bool isCustomer = true;

                foreach (var user in UserStatic.ConnectedUsers.ToList())
                {
                    if (user.ConnID == sg.Value.UserConnId && user.UserID == sg.Value.GroupName)
                    {
                        isCustomer = user.Role == "customer";
                    }
                }

                return isCustomer && sg.Value.AdminConnId == connId && sg.Value.GroupName != User.Identity.Name;
            })
                .Select(sg =>
            {
                Member member = dbContext.Members.SingleOrDefault(m => m.ID.ToString() == sg.Value.GroupName);

                int preMsgCount = dbContext.GroupChats.Count(gc => gc.GroupID.ToString() == sg.Key 
                    && gc.Group.IsAlive && gc.Group.IsService);

                return new
                {
                    ImagePath = member.Image,
                    GroupID = sg.Key,
                    UserName = member.UserName,
                    PreMsgCount = preMsgCount
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

            Member member = dbContext.Members
                .SingleOrDefault(m => m.ID.ToString() == User.Identity.Name);

            if (member.IsAdmin)
            {
                var Context = GlobalHost.ConnectionManager.GetHubContext<ChatHub>();

                foreach (var groupId in UserStatic.ServiceGroups.Keys.ToList())
                {
                    if (UserStatic.ServiceGroups[groupId].AdminConnId == "" 
                        && UserStatic.ServiceGroups[groupId].GroupName != User.Identity.Name)
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
            //===================================================
            //ServiceNotRead
            string user = UserStatic.ServiceGroups[groupId].GroupName;

            if (UserStatic.ServiceNotRead.ContainsKey(user)
                && UserStatic.ServiceNotRead[user].ContainsKey(groupId))
            {
                UserStatic.ServiceNotRead[user][groupId]++;
            }
            else if (UserStatic.ServiceNotRead.ContainsKey(user)
                && !UserStatic.ServiceNotRead[user].ContainsKey(groupId))
            {
                UserStatic.ServiceNotRead[user].Add(groupId, 1);
            }
            else
            {
                UserStatic.ServiceNotRead.Add(user, new Dictionary<string, int>());
                UserStatic.ServiceNotRead[user].Add(groupId, 1);
            }
            //==================================================================

            var Context = GlobalHost.ConnectionManager.GetHubContext<ChatHub>();

            DateTime timeStamp = DateTime.Now;

            Context.Clients.Group(groupId)
                .receive(connId, "客服人員", message, timeStamp.ToString("M/d HH:mm"), 
                    "e9ec5c93-c442-4d6d-96d1-fc2fb8c570fcuser2.png", groupId, UserStatic.ServiceNotRead[user][groupId]);

            
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
            var msgList = dbContext.GroupChats.Where(gc => gc.GroupID.ToString() == groupId
                 && gc.Group.IsAlive && gc.Group.IsService)
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

                        UserDetail admin = UserStatic.ConnectedUsers
                            .SingleOrDefault(cu => cu.ConnID == UserStatic.ServiceGroups[groupId].AdminConnId
                                && cu.UserID == gc.MemberID.ToString());

                        UserDetail customer = UserStatic.ConnectedUsers
                            .SingleOrDefault(cu => cu.ConnID == UserStatic.ServiceGroups[groupId].UserConnId
                                && cu.UserID == gc.MemberID.ToString());

                        if (admin != null || customer == null)
                        {
                            connId = UserStatic.ServiceGroups[groupId].AdminConnId;
                            userName = "客服人員";
                            image = "e9ec5c93-c442-4d6d-96d1-fc2fb8c570fcuser2.png";
                        }
                        else
                        {
                            Member member = dbContext.Members.SingleOrDefault(m => m.ID == gc.MemberID);
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