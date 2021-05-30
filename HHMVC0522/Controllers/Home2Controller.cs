using BLL;
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
                }
            }
            else if (model.Comment.Title != null && model.Comment.CommentContent != null)
            {
                if (postBLL.UpdateComment(model))
                {
                    ViewData["CommentState"] = "Success";
                    ModelState.Clear();
                }
                else
                {
                    ViewData["CommentState"] = "Error";
                }
            }
            else
            {
                ViewData["CommentState"] = "Error";
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

        public ActionResult UpdateComment(int ID, string title, string content, int postID)
        {
            CommentDTO commentDTO = new CommentDTO();
            commentDTO.ID = ID;
            commentDTO.Title = title;
            commentDTO.CommentContent = content;
            if (commentDTO.Title != null && commentDTO.CommentContent != null)
            {

                if (commentBLL.UpdateComment(commentDTO))
                {
                    ViewData["CommentState"] = "Success";
                    ModelState.Clear();
                }
                else
                {
                    ViewData["CommentState"] = "Error";
                }
            }
            else
            {
                ViewData["CommentState"] = "Error";
            }
            return RedirectToAction("PostDetail/" + postID, "Home2");

        }
    }
}