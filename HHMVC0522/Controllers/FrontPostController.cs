using BLL;
using DTO;
using PagedList;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace UI.Controllers
{
    public class FrontPostController : Controller
    {
        // GET: FrontPost
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult UserPostList()
        {
            List<PostDTO> postList = postBLL.GetUserPosts(UserStatic.UserID);
            return View(postList);
        }
        PostBLL postBLL = new PostBLL();
        public ActionResult AddPost()
        {
            PostDTO postDTO = new PostDTO();
            postDTO.Categories = PostCategoryBLL.GetPostCategoriesForDropDown();
            return View(postDTO);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult AddPost(PostDTO model)
        {
            if (model.PostImage[0] == null)
            {
                ViewBag.ProcessState = General.Messages.ImageMissing;
                model.Categories = PostCategoryBLL.GetPostCategoriesForDropDown();
                return View(model);
            }
            else if (ModelState.IsValid)
            {
                foreach (var item in model.PostImage)
                {
                    string ext = Path.GetExtension(item.FileName);
                    if (ext != ".png" && ext != ".jpg" && ext != ".jpeg")
                    {
                        ViewBag.ProcessState = General.Messages.ExtensionError;
                        model.Categories = PostCategoryBLL.GetPostCategoriesForDropDown();
                        return View(model);
                    }
                }
                List<PostImageDTO> imageList = new List<PostImageDTO>();
                foreach (HttpPostedFileBase postedFile in model.PostImage)
                {
                    Bitmap image = new Bitmap(postedFile.InputStream);
                    Bitmap resizedImage = new Bitmap(image, 740, 416);
                    string uniqueNumber = Guid.NewGuid().ToString();
                    string fileName = uniqueNumber + postedFile.FileName;
                    resizedImage.Save(Server.MapPath("~/Areas/Admin/Content/PostImages/" + fileName));
                    PostImageDTO dto = new PostImageDTO();
                    dto.ImagePath = fileName;
                    imageList.Add(dto);
                }
                model.PostImages = imageList;
                if (postBLL.AddPost(model))
                {
                    ViewData["CommentState"] = "Success";
                    ModelState.Clear();
                    ViewBag.ProcessState = General.Messages.AddSuccess;

                    model = new PostDTO();
                }
                else
                {
                    ViewBag.ProcessState = General.Messages.GeneralError;
                    model.Categories = PostCategoryBLL.GetPostCategoriesForDropDown();
                    return View(model);
                }
            }
            else
            {
                ViewBag.ProcessState = General.Messages.EmptyArea;
                model.Categories = PostCategoryBLL.GetPostCategoriesForDropDown();
                return View(model);
            }
            return RedirectToAction("Index", "Home2");
        }
        public ActionResult UpdatePost(int ID)
        {
            PostDTO model = new PostDTO();
            model = postBLL.GetPostWithID(ID);
            model.Categories = PostCategoryBLL.GetPostCategoriesForDropDown();
            model.IsUpdate = true;
            return View(model);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult UpdatePost(PostDTO model)
        {
            IEnumerable<SelectListItem> selectList = PostCategoryBLL.GetPostCategoriesForDropDown();
            if (ModelState.IsValid)
            {
                if (model.PostImage[0] != null)
                {
                    foreach (var item in model.PostImage)
                    {
                        string ext = Path.GetExtension(item.FileName);
                        if (ext != ".png" && ext != ".jpg" && ext != ".jpeg")
                        {
                            ViewBag.ProcessState = General.Messages.ExtensionError;
                            model.Categories = PostCategoryBLL.GetPostCategoriesForDropDown();
                            return View(model);
                        }
                    }
                    List<PostImageDTO> imageList = new List<PostImageDTO>();
                    foreach (HttpPostedFileBase postedFile in model.PostImage)
                    {
                        Bitmap image = new Bitmap(postedFile.InputStream);
                        Bitmap resizedImage = new Bitmap(image, 740, 416);
                        string uniqueNumber = Guid.NewGuid().ToString();
                        string fileName = uniqueNumber + postedFile.FileName;
                        resizedImage.Save(Server.MapPath("~/Areas/Admin/Content/PostImages/" + fileName));
                        PostImageDTO dto = new PostImageDTO();
                        dto.ImagePath = fileName;
                        imageList.Add(dto);
                    }
                    model.PostImages = imageList;

                }
                if (postBLL.UpdatePost(model))
                {
                    ViewBag.ProcessState = General.Messages.UpdateSuccess;
                }
                else
                {
                    ViewBag.ProcessState = General.Messages.GeneralError;
                }

            }
            else
            {
                ViewBag.ProcessState = General.Messages.EmptyArea;
            }
            model = postBLL.GetPostWithID(model.ID);
            model.Categories = selectList;
            model.IsUpdate = true;
            return RedirectToAction("PostDetail/" + model.ID, "Home2");
        }
        int pageSize = 5;
        public ActionResult NewsList(int page = 1)
        {
            int currentPage = page < 1 ? 1 : page;
            List<PostDTO> newsList = postBLL.GetNews();
            var result = newsList.ToPagedList(currentPage, pageSize);
            return View(result);
        }
        public ActionResult AllPosts(int page = 1)
        {
            int currentPage = page < 1 ? 1 : page;
            List<PostDTO> postList = postBLL.GetPosts();
            var result = postList.ToPagedList(currentPage, pageSize);
            return View(result);
        }
        public ActionResult DeletePostImage(int ID)
        {
            string imagePath = postBLL.DeletePostImage(ID);
            string imageFullPath = Server.MapPath("~/Areas/Admin/Content/PostImages/" + imagePath);
            if (System.IO.File.Exists(imageFullPath))
            {
                System.IO.File.Delete(imageFullPath);
            }
            return RedirectToAction("Index", "Home2");
        }
        public ActionResult DeletePost(int ID)
        {
            List<PostImageDTO> imageList = postBLL.DeletePost(ID);
            foreach (PostImageDTO item in imageList)
            {
                string imageFullPath = Server.MapPath("~/Areas/Admin/Content/PostImages/" + item.ImagePath);
                if (System.IO.File.Exists(imageFullPath))
                {
                    System.IO.File.Delete(imageFullPath);
                }
            }
            return RedirectToAction("Index", "Home2");
        }
        public JsonResult GetSearchPost(string SearchCategory, string SearchText)
        {
            List<PostDTO> postList = new List<PostDTO>();
            postList = postBLL.GetPosts(Int32.Parse(SearchCategory), SearchText);
            return Json(postList, JsonRequestBehavior.AllowGet);
        }
    }
}