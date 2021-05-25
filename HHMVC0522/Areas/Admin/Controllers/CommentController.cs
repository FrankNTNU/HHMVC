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
            List<CommentDTO> commentList = commentBLL.GetAllComments();
            return View(commentList);
        }
    }
}