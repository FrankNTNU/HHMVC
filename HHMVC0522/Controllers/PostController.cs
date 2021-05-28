using BLL;
using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace UI.Controllers
{
    public class PostController : Controller
    {
        // GET: Post
        public ActionResult Index()
        {
            return View();
        }
        PostBLL postBLL = new PostBLL();
        public ActionResult AddPost()
        {
            PostDTO postDTO = new PostDTO();
            return View(postDTO);
        }
        [HttpPost]
        public ActionResult AddPost(PostDTO postDTO)
        {
            postBLL.AddPost(postDTO);
            return RedirectToAction("Index", "Home2");
        }
    }
}