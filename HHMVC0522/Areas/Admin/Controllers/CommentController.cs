using BLL;
using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace UI.Areas.Admin.Controllers
{
    public class CommentController : BaseController
    {
        // GET: Admin/Comment
        public ActionResult Index()
        {
            return View();
        }
        CommentBLL commentBLL = new CommentBLL();
        public ActionResult AllComments()
        {
            List<CommentDTO> commentList = new List<CommentDTO>();
            if (UserStatic.isAdmin)
            {
                commentList = commentBLL.GetAllComments();

            }
            else
            {
                commentList = commentBLL.GetAllComments(UserStatic.UserID);
            }
            return View(commentList);
        }
        [Authorize(Users = "admin")]
        public ActionResult UnapprovedComments()
        {
            List<CommentDTO> commentList = commentBLL.GetUnapprovedComments();
            return View(commentList);
        }
        public ActionResult ApproveComment(int ID)
        {
            commentBLL.ApproveComment(ID);
            return RedirectToAction("AllComments", "Comment");
        }
        public JsonResult DeleteComment(int ID)
        {
            commentBLL.DeleteComment(ID);
            return Json("");
        }
    }
}