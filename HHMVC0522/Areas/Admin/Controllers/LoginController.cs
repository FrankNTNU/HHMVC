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
                    UserStatic.UserID = user.ID;
                    UserStatic.isAdmin = user.IsAdmin;
                    UserStatic.NameSurname = user.Name;
                    UserStatic.ImagePath = user.ImagePath;
                    UserStatic.StatusID = user.StatusID;
                    if (UserStatic.isAdmin)
                    {
                        FormsAuthentication.RedirectFromLoginPage("admin", true);
                        return RedirectToAction("UserList", "User");
                    }
                    else
                    {
                        //FormsAuthentication.RedirectFromLoginPage("regular", true);
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