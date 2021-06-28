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
        public JsonResult GetServiceGroup(string connId)
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
        public JsonResult AddToServiceGroup(string connId)
        {
            foreach (var groupId in UserStatic.ServiceGroups.Keys.ToList())
            {

            }

            return Json(new { Result = "Succes" });
        }
    }
}