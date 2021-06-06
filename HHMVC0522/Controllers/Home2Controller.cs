using BLL;
using DAL;
using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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

            //========================================================
            //恩旗
            //determine if last 7 days no weight log
            HealthHelperEntities dbContext = new HealthHelperEntities();
            DateTime today = DateTime.Now.Date;
            DateTime d7b = today.AddDays(-7);
            var q1 = dbContext.WeightLogs.Where(wgt => wgt.UpdatedDate < today && wgt.UpdatedDate >= d7b);
            List<WeightLog> list = q1.ToList();
            if (list.Count == 0)
            {
                Session["NoWeightLog"] = true;
            }
            else
            {
                Session["NoWeightLog"] = false;
            }
            //=========================================================

            return View(layoutDTO);
        }
        public ActionResult Login()
        {
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
                    UserStatic.UserID = user.ID;
                    UserStatic.IsAdmin = user.IsAdmin;
                    UserStatic.NameSurname = user.Name;
                    UserStatic.ImagePath = user.ImagePath;
                    UserStatic.StatusID = user.StatusID;
                    UserStatic.Points = user.Points;

                    return RedirectToAction("Index", "Home2");
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
            int id = UserStatic.UserID;
            UserStatic.UserID = 0;
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

      
    }
}