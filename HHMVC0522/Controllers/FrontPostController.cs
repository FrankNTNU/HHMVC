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
        private static int currentPostID;
        // GET: FrontPost
        public ActionResult Index()
        {
            return View();
        }
        LayoutBLL layoutBLL;
        public static int level = 0;
        public ActionResult PostDetail(int ID)
        {
            layoutBLL = new LayoutBLL();
            LayoutDTO layoutDTO = new LayoutDTO();
            currentPostID = ID;
            postBLL.AddViewCount(ID);
            layoutDTO = layoutBLL.GetPostDetailPageItemWithID(ID);
            return View(layoutDTO);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult PostDetail(LayoutDTO model)
        {
            layoutBLL = new LayoutBLL();
            if (model.Comment.Name != null && model.Comment.Title != null && model.Comment.CommentContent != null)
            {
                if (postBLL.AddComment(model))
                {
                    TempData["State"] = "AddSuccess";
                    ModelState.Clear();
                }
                else
                {
                    TempData["State"] = "Error";
                    ViewBag.ProcessState = General.Messages.GeneralError;
                }
            }
            else
            {
                TempData["State"] = "Error";
                ViewBag.ProcessState = General.Messages.EmptyArea;

            }
            LayoutDTO layoutDTO = new LayoutDTO();
            layoutDTO = layoutBLL.GetPostDetailPageItemWithID(model.PostDetail.ID);
            return View(layoutDTO);
        }
        [Authorize]
        public ActionResult UserPostList(int page = 1)
        {
            if (Session["ID"] == null)
                return Redirect("~/Home2/Index");
            int currentPage = page < 1 ? 1 : page;
            postListResult = postBLL.GetUserPosts((int)Session["ID"]);
            var result = postListResult.ToPagedList(currentPage, pageSize);
            return View(result);
            //return RedirectToAction("ShowPosts");
        }
        PostBLL postBLL = new PostBLL();
        [Authorize]
        public ActionResult AddPost()
        {
            if (Session["ID"] == null)
                return Redirect("~/Home2/Index");
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
                TempData["State"] = "ImageMissing";
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
                        TempData["State"] = "ExtensionError";
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
                model.CategoryID = General.Category.UserPost;
                model.IsApproved = false;
                if (postBLL.AddPost(model))
                {
                    ModelState.Clear();
                    ViewBag.ProcessState = General.Messages.AddSuccess;
                    TempData["State"] = "AddPostSuccess";
                    model = new PostDTO();
                }
                else
                {
                    ViewBag.ProcessState = General.Messages.GeneralError;
                    model.Categories = PostCategoryBLL.GetPostCategoriesForDropDown();
                    TempData["State"] = "Error";
                    return View(model);
                }
            }
            else
            {
                ViewBag.ProcessState = General.Messages.EmptyArea;
                model.Categories = PostCategoryBLL.GetPostCategoriesForDropDown();
                TempData["State"] = "EmptyArea";
                return View(model);
            }
            return RedirectToAction("Index", "Home2");
        }
        private static int categoryID;
        public ActionResult UpdatePost(int ID)
        {
            PostDTO model = new PostDTO();
            model = postBLL.GetPostWithID(ID);
            model.Categories = PostCategoryBLL.GetPostCategoriesForDropDown();
            model.IsUpdate = true;
            categoryID = model.CategoryID;
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
                model.CategoryID = categoryID;
                model.IsApproved = false;
                if (postBLL.UpdatePost(model))
                {
                    TempData["State"] = "UpdateSuccess";
                    ViewBag.ProcessState = General.Messages.UpdateSuccess;
                    return RedirectToAction("PostDetail/" + model.ID, "FrontPost");
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
        int pageSize = 5;
        static List<PostDTO> postListResult = new List<PostDTO>();
      
        public ActionResult ShowPosts(int page = 1)
        {
            int currentPage = page < 1 ? 1 : page;
            var result = postListResult.ToPagedList(currentPage, pageSize);
            return View(result);
        }
        public ActionResult AllPosts()
        {
            postListResult = postBLL.GetPosts();
            return RedirectToAction("ShowPosts");
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
            TempData["State"] = "DeleteSuccess";
            return RedirectToAction("Index", "Home2");
        }
        public ActionResult GetPostsWithSearchText(string SearchCategory, string SearchText)
        {
            if (SearchText == "" && SearchCategory != "0")
            {
                postListResult = postBLL.GetPostsByCategory(Int32.Parse(SearchCategory));
            }
            else if (SearchText == "" && SearchCategory == "0") // Search All.
            {
                postListResult = postBLL.GetPosts();
            }
            else
            {
                postListResult = postBLL.GetPosts(Int32.Parse(SearchCategory), SearchText);
            }
            

            return RedirectToAction("ShowPosts");
        }
       
        public string Like(int postID, int number)
        {
            if (!postBLL.HasLiked((int)Session["ID"], postID) && number > 0) 
                // Hasn't liked the post and want to like the post.
            {
                postBLL.LikePost(postID, number);
                return "true";
            }
            else if (postBLL.HasLiked((int)Session["ID"], postID) && number < 0)
            {
                postBLL.LikePost(postID, number);
                return "true";
            }
            // Has liked the post and want to take the like away.
            return "false";
        }
        public string HasLiked(int postID)
        {
            if (postBLL.HasLiked((int)Session["ID"], postID)) return "true";
            else return "false";
        }
        CommentBLL commentBLL = new CommentBLL();
        public ActionResult UpdateComment(int commentID)
        {
            CommentDTO model = new CommentDTO();
            model = commentBLL.GetComment(commentID);
            ViewBag.CurrentPostID = currentPostID;
            return View(model);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult UpdateComment(CommentDTO model)
        {
            if (ModelState.IsValid)
            {
                if (commentBLL.UpdateComment(model))
                {
                    TempData["State"] = "UpdateSuccess";
                    ModelState.Clear();
                    return RedirectToAction("PostDetail/" + currentPostID, "FrontPost");

                }
                else
                {
                    TempData["State"] = "Error";
                    Session["PostID"] = currentPostID;
                    return View(model);
                }
            }
            else
            {
                TempData["State"] = "Error";
                Session["PostID"] = currentPostID;
                return View(model);
            }
            
        }
        public ActionResult DeleteComment(int ID, int postID)
        {
            //commentBLL.DeleteComment(ID);
            commentBLL.HideComment(ID);
            TempData["State"] = "DeleteSuccess";
            ModelState.Clear();
            return RedirectToAction("PostDetail/" + postID, "FrontPost");
        }
        public ActionResult ReplyToComment(int commentID)
        {
            CommentDTO model = new CommentDTO();
            parentCommentID = commentID;
            ViewBag.CurrentPostID = currentPostID;
            return View(model);
        }
        static int parentCommentID;
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ReplyToComment(CommentDTO model)
        {
            if (ModelState.IsValid)
            {
                model.MemberID = (int)Session["ID"];
                model.ParentCommentID = parentCommentID;
                postBLL.AddReply(model);
                TempData["State"] = "ReplySuccess";
                return RedirectToAction("PostDetail/" + currentPostID, "FrontPost");
            }
            else
            {
                ViewBag.ProcessState = General.Messages.EmptyArea;
                return View(model);
            }
        }
        public string ReportComment(int commentID)
        {
            commentBLL.ReportComment(commentID);
            TempData["State"] = "ReportSent";
            return "";
        }
    }
}