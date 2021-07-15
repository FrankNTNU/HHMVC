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
            commentBLL = new CommentBLL();
            commentList = commentBLL.GetAllComments();
            return View(commentList);
        }
        
        public ActionResult UnapprovedComments()
        {
            commentBLL = new CommentBLL();
            List<CommentDTO> commentList = commentBLL.GetUnapprovedComments();
            return View(commentList);
        }
        public ActionResult ApproveComment(int ID)
        {
            commentBLL.ApproveComment(ID);
            return RedirectToAction("AllComments", "Comment");
        }
        public ActionResult HideComment(int ID)
        {
            commentBLL.HideComment(ID);
            return RedirectToAction("AllComments", "Comment");
        }
        public JsonResult DeleteComment(int ID)
        {
            commentBLL.DeleteComment(ID);
            return Json("");
        }
        public ActionResult ClearReport(int commentID)
        {
            commentBLL.ClearReport(commentID);
            return RedirectToAction("AllComments", "Comment");
        }
    }
}