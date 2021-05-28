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
    public class UserController : BaseController
    {
        // GET: Admin/User
        UserBLL userBLL = new UserBLL();
        [Authorize(Users = "admin")]
        public ActionResult UserList()
        {
            var user = User.Identity.Name;
            List<UserDTO> model = new List<UserDTO>();
            model = userBLL.GetUsers();
            return View(model);
        }
        public ActionResult AddUser()
        {
            UserDTO model = new UserDTO();
            model.Statuses = StatusBLL.GetStatusesForDropDown();
            model.ActivityLevels = ActivityLevelBLL.GetActivityLevelsForDropDown();
            return View(model);
        }
        [HttpPost]
        public ActionResult AddUser(UserDTO model)
        {
            if (model.UserImage == null)
            {
                ViewBag.ProcessState = General.Messages.ImageMissing;
            }
            else if (ModelState.IsValid)
            {
                HttpPostedFileBase postedFile = model.UserImage;
                string ext = Path.GetExtension(postedFile.FileName);
                if (ext == ".jpg" || ext == ".jpeg" || ext == ".png" || ext == ".gif")
                {
                    Bitmap userImage = new Bitmap(postedFile.InputStream);
                    Bitmap resizedImage = new Bitmap(userImage, 128, 128);
                    string uniqueNumber = Guid.NewGuid().ToString();
                    string fileName = uniqueNumber + postedFile.FileName;
                    model.ImagePath = fileName;
                    userBLL.AddUser(model);
                    ViewBag.ProcessState = General.Messages.AddSuccess;
                    resizedImage.Save(Server.MapPath("~/Areas/Admin/Content/UserImage/" + fileName));
                    ModelState.Clear();
                    model = new UserDTO();
                }
                else
                {
                    ViewBag.ProcessState = General.Messages.ExtensionError;
                }
            }
            else
            {
                ViewBag.ProcessState = General.Messages.GeneralError;
            }
            model.ActivityLevels = ActivityLevelBLL.GetActivityLevelsForDropDown();
            model.Statuses = StatusBLL.GetStatusesForDropDown();
            userBLL = new UserBLL();
            return View(model);
        }
        public ActionResult UpdateUser(int ID)
        {
            UserDTO dto = new UserDTO();
            dto = userBLL.GetUserWithID(ID);
            dto.Statuses = StatusBLL.GetStatusesForDropDown();
            dto.ActivityLevels = ActivityLevelBLL.GetActivityLevelsForDropDown();
            return View(dto);
        }
        [HttpPost]
        [Authorize(Users = "admin")]
        public ActionResult UpdateUser(UserDTO model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.ProcessState = General.Messages.EmptyArea;
            }
            else
            {
                if (model.UserImage != null) // Image has been changed.
                {
                    HttpPostedFileBase postedFile = model.UserImage;
                    string ext = Path.GetExtension(postedFile.FileName);
                    if (ext == ".jpg" || ext == ".jpeg" || ext == ".png" || ext == ".gif")
                    {
                        Bitmap userImage = new Bitmap(postedFile.InputStream);
                        if (Math.Abs(((double)userImage.Height / userImage.Width) - 1) > 0.25)
                        {
                            ViewBag.ProcessState = General.Messages.WrongImageSize;
                            model.Statuses = StatusBLL.GetStatusesForDropDown();
                            model.ActivityLevels = ActivityLevelBLL.GetActivityLevelsForDropDown();
                            return View(model);
                        }
                        Bitmap resizedImage = new Bitmap(userImage, 256, 256);
                        string uniqueNumber = Guid.NewGuid().ToString();
                        string fileName = uniqueNumber + postedFile.FileName;
                        resizedImage.Save(Server.MapPath("~/Areas/Admin/Content/UserImage/" + fileName));
                        model.ImagePath = fileName;
                    }
                    string oldImagePath = userBLL.UpdateUser(model);
                    if (model.UserImage != null)
                    {
                        string oldImageFullPath = "~/Areas/Admin/Content/UserImage/" + oldImagePath;
                        if (System.IO.File.Exists(Server.MapPath(oldImageFullPath)))
                        {
                            System.IO.File.Delete(Server.MapPath(oldImageFullPath));
                        }
                        ViewBag.ProcessState = General.Messages.UpdateSuccess;
                        userBLL = new UserBLL();
                    }
                }
                else
                {
                    userBLL.UpdateUser(model);
                    userBLL = new UserBLL();
                    ViewBag.ProcessState = General.Messages.UpdateSuccess;
                }
            }
            model.Statuses = StatusBLL.GetStatusesForDropDown();
            model.ActivityLevels = ActivityLevelBLL.GetActivityLevelsForDropDown();
            if (UserStatic.UserID == model.ID) // Updated yourself
            {
                UserStatic.NameSurname = model.Name;
                UserStatic.ImagePath = model.ImagePath;
            }
            return View(model);
        }
        WeightLogBLL weightLogBLL = new WeightLogBLL();
        WorkoutLogBLL workoutLogBLL = new WorkoutLogBLL();
        DietLogBLL dietLogBLL = new DietLogBLL();
        CommentBLL commentBLL = new CommentBLL();
        public JsonResult DeleteUser(int ID)
        {
            weightLogBLL.DeleteByMemberID(ID);
            workoutLogBLL.DeleteByMemberID(ID);
            dietLogBLL.DeleteByMemberID(ID);
            commentBLL.DeleteByMemberID(ID);
            string imagePath = userBLL.DeleteUser(ID);
            string ImageFullPath = Server.MapPath(@"~\Areas\Admin\Content\UserImage\" + imagePath);
            if (System.IO.File.Exists(ImageFullPath))
            {
                System.IO.File.Delete(ImageFullPath);
            }
            userBLL = new UserBLL();
            return Json("");
        }
    }
}