using BLL;
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
            return Json(UserStatic.ConnectedUsers,JsonRequestBehavior.AllowGet);
        }
        public JsonResult HasChanged(IEnumerable<UserDetail> previousList)
        {
            if (UserStatic.ConnectedUsers != previousList)
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
    }
}