using BLL;
using DAL;
using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace UI.Controllers
{
    public class Home2Controller : Controller
    {
        // GET: Home2
        LayoutBLL layoutBLL = new LayoutBLL();
        PostBLL postBLL = new PostBLL();
        UserBLL userBLL = new UserBLL();
        public ActionResult Index()
        {
            LayoutDTO layoutDTO = new LayoutDTO();
            layoutDTO = layoutBLL.GetPosts();

            //==========================
            //恩旗
            //if (Session["ID"] != null)
            //{
            //    SetWeightLogSession();
            //}
            //==========================

            return View(layoutDTO);
        }

        public ActionResult Login(string ReturnUrl)
        {
            ViewBag.ReturnUrl = ReturnUrl;
            UserDTO dto = new UserDTO();
            return View(dto);
        }

        [HttpPost]
        public ActionResult Login(UserDTO model)
        {
            if (model.UserName != null && model.Password != null)
            {
                UserDTO user = userBLL.GetUserWithUsernameAndPassword(model);
                if (user.ID != 0)
                {
                    Session["ID"] = user.ID;
                    Session["Name"] = user.Name;
                    Session["ImagePath"] = user.ImagePath;
                    Session["UserName"] = user.UserName;

                    //====================================
                    //恩旗
                    //Use RedirectFromLoginPage to redirect
                    FormsAuthentication.RedirectFromLoginPage(user.ID.ToString(), false);
                    
                    //var i = User.Identity.Name;
                    //return RedirectToAction("Index", "Home2");
                    return null;
                    //====================================
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
        public ActionResult Logout()
        {
            Session.Clear();
            FormsAuthentication.SignOut();
            return RedirectToAction("Index");
        }
        public ActionResult PostDetail(int ID)
        {
            layoutBLL = new LayoutBLL();
            LayoutDTO layoutDTO = new LayoutDTO();
            layoutDTO = layoutBLL.GetPostDetailPageItemWithID(ID);
            return View(layoutDTO);
        }

        [HttpPost]
        [ValidateInput(false)]

        public ActionResult PostDetail(LayoutDTO model)
        {

            if (model.Comment.Name != null && model.Comment.Title != null && model.Comment.CommentContent != null)
            {
                if (postBLL.AddComment(model))
                {
                    ViewData["CommentState"] = "Success";
                    ModelState.Clear();
                }
                else
                {
                    ViewData["CommentState"] = "Error";
                    ViewBag.ProcessState = General.Messages.GeneralError;

                }
            }
           
            else
            {
                ViewData["CommentState"] = "Error";
                ViewBag.ProcessState = General.Messages.EmptyArea;

            }
            LayoutDTO layoutDTO = new LayoutDTO();
            layoutDTO = layoutBLL.GetPostDetailPageItemWithID(model.PostDetail.ID);
            return View(layoutDTO);
        }
        CommentBLL commentBLL = new CommentBLL();
        public ActionResult DeleteComment(int ID, int postID)
        {
            commentBLL.DeleteComment(ID);
            ViewData["CommentState"] = "Success";
            ModelState.Clear();
            return RedirectToAction("PostDetail/" + postID, "Home2");
        }

        //========================================================
        //恩旗
        //For dialog
        [HttpPost]
        public void SetDialogShownSession() {
            Session["dialogShown"] = true;
        }

        //determine if last 7 days has no weight log
        [NonAction]
        public static void SetWeightLogSession(HttpContextBase context)
        {
            HealthHelperEntities dbContext = new HealthHelperEntities();
            DateTime today = DateTime.Now;
            DateTime d7b = DateTime.Now.Date.AddDays(-7);

            int MemberID = (int)context.Session["ID"];

            var q1 = dbContext.WeightLogs.Where(wgt => wgt.MemberID == MemberID
                && wgt.UpdatedDate <= today && wgt.UpdatedDate >= d7b);
            List<WeightLog> list = q1.ToList();
            if (list.Count == 0)
            {
                context.Session["NoWeightLog"] = true;
            }
            else
            {
                context.Session["NoWeightLog"] = false;
            }
        }

        //determine if there is any WorkoutPreferences
        [NonAction]
        public static void SetPreferencesSession(HttpContextBase context)
        {
            HealthHelperEntities dbContext = new HealthHelperEntities();

            int MemberID = (int)context.Session["ID"];

            var q1 = dbContext.WorkoutPreferences.Where(wp => wp.MemberID == MemberID);

            if (!q1.Any())
            {
                context.Session["NoPreferences"] = true;
            }
            else
            {
                context.Session["NoPreferences"] = false;
            }
        }

        //=========================================================
    }
}