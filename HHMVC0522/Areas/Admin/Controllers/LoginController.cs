using DTO;
using BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Threading;

namespace UI.Areas.Admin.Controllers
{
    public class LoginController : Controller
    {
        // GET: Admin/Login
        UserBLL userBLL = new UserBLL();
        public ActionResult Index()
        {
            UserDTO dto = new UserDTO();
            return View(dto);
        }
        [AllowAnonymous]
        [HttpPost]
        public ActionResult Index(UserDTO model)
        {
            if (model.UserName != null && model.Password != null)
            {
                UserDTO user = userBLL.GetUserWithUsernameAndPassword(model);
                if (user.ID != 0)
                {
                    Session["name"] = user.Name;
                    Session["ImagePath"] = user.ImagePath;
                    Session["IsAdmin"] = user.IsAdmin;
                    Session["ID"] = user.ID;
                    if (user.IsAdmin)
                    {
                        FormsAuthentication.RedirectFromLoginPage(user.ID.ToString(), false);
                        return RedirectToAction("UserList", "User");
                    }
                    else
                    {
                        ViewBag.ProcessState = General.Messages.NotAdmin;
                        return View(model);
                    }
                }
                else
                {
                    ViewBag.ProcessState = General.Messages.LoginError;
                    return View(model);
                }
            }
            else
            {
                return View(model);
            }

        }
    }
}