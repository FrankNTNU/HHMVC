using BLL;
using DTO;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace UI.Areas.Admin.Controllers
{
    public class PostController : BaseController
    {
        // GET: Admin/Post
        PostBLL postBLL = new PostBLL();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult PostList()
        {
            //CountDTO countDTO = new CountDTO();
            //countDTO = bll.GetAllCounts();
            //ViewData["AllCounts"] = countDTO;
            List<PostDTO> postList = new List<PostDTO>();
            postList = postBLL.GetPosts();
            return View(postList);
        }
        public ActionResult AddPost()
        {
            PostDTO model = new PostDTO();
            model.Categories = PostCategoryBLL.GetPostCategoriesForDropDown();
            return View(model);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult AddPost(PostDTO model)
        {
            if (model.PostImage[0] == null)
            {
                ViewBag.ProcessState = General.Messages.ImageMissing;
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
                    Bitmap image = new Bitmap(postedFile.InputStream); // 把上傳圖片轉成Bitmap
                    Bitmap resizedImage = new Bitmap(image, 740, 416); // 設定長寬
                    string uniqueNumber = Guid.NewGuid().ToString(); // 設定唯一字串
                    string fileName = uniqueNumber + postedFile.FileName; // 圖片路徑  = 唯一字串 + 圖片檔名
                    resizedImage.Save(Server.MapPath("~/Areas/Admin/Content/PostImages/" + fileName)); // 存在資料夾
                    PostImageDTO dto = new PostImageDTO();
                    dto.ImagePath = fileName; // 把圖片路徑存在DTO的屬性
                    imageList.Add(dto);
                }
                model.PostImages = imageList;
                if (postBLL.AddPost(model))
                {
                    ViewBag.ProcessState = General.Messages.AddSuccess;
                    ModelState.Clear();
                    model = new PostDTO();
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
            model.Categories = PostCategoryBLL.GetPostCategoriesForDropDown();
            return View(model);
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
            return View(model);
        }
        public JsonResult DeletePostImage(int ID)
        {
            string imagePath = postBLL.DeletePostImage(ID);
            string imageFullPath = Server.MapPath("~/Areas/Admin/Content/PostImages/" + imagePath);
            if (System.IO.File.Exists(imageFullPath))
            {
                System.IO.File.Delete(imageFullPath);
            }
            return Json("");
        }
        public JsonResult DeletePost(int ID)
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
            return Json("");
        }
    }
}