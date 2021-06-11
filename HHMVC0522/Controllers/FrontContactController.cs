using BLL;
using DAL;
using DTO;
using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace UI.Controllers
{
    public class FrontContactController : Controller
    {
        // GET: FrontContact
        public ActionResult Index()
        {
            return View();
        }
      
        public ActionResult Chat()
        {
            if(Session["ID"] == null)
            {
                return RedirectToAction("Index", "Home2");
            }
            return View();
        }


        [HttpPost]
        public ActionResult Chat(string message)
        {
            HealthHelperEntities dbContext = new HealthHelperEntities();
            Random rn = new Random();

            List<string> connIds = UserStatic.ConnectedUsers.Select(cu => new
            {
                cu.ConnID,
                UserID = int.Parse(cu.UserID)
            }).Where(cu => dbContext.Members.SingleOrDefault(cu1 => cu1.ID == cu.UserID).IsAdmin)
                .Select(cu => cu.ConnID).ToList();


            var context = GlobalHost.ConnectionManager.GetHubContext<ChatHub>();

            context.Clients.Client(connIds[rn.Next(0, connIds.Count)])
                .ReceiveFromCustomer(Session["Name"], Session["ImagePath"], message);

            return View();
        }
    }
}