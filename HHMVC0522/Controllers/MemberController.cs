using BLL;
using DAL;
using DTO;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;


namespace UI.Controllers
{
    public class MemberController : Controller
    {
        // GET: Member
        MemberBLL memberBLL=new MemberBLL();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult CreateMember()
        {
            MemberDTO model = new MemberDTO();
            model.Statuses = StatusBLL.GetStatusesForDropDown();
            model.ActivityLevels = ActivityLevelBLL.GetActivityLevelsForDropDown();
            return View(model);
        }
        [HttpPost]
        public ActionResult CreateMember(MemberDTO model)
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
                    int userID =memberBLL.RegisterUser(model);
                    ViewBag.ProcessState = General.Messages.AddSuccess;
                    resizedImage.Save(Server.MapPath("~/Areas/Admin/Content/UserImage/" + fileName));
                    ModelState.Clear();
                    
                    string activeCode = memberBLL.GetActiveCode(userID);
                    SendConfirmEmail(activeCode);
                    return RedirectToAction("LoginFirstTime","Home2");
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
            memberBLL = new MemberBLL();
            return View(model);
        }

        public ActionResult EditMember(int ID)
        {
            MemberDTO dto = new MemberDTO();
            dto = memberBLL.GetMemberWithID(ID);
            dto.Statuses = StatusBLL.GetStatusesForDropDown();
            dto.ActivityLevels = ActivityLevelBLL.GetActivityLevelsForDropDown();
            ViewBag.IsUpdate = "true";
            return View(dto);
        }
        [HttpPost]
        public ActionResult EditMember(MemberDTO model)
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
                    string oldImagePath = memberBLL.EditMember(model);
                    if (model.UserImage != null)
                    {
                        string oldImageFullPath = "~/Areas/Admin/Content/UserImgs/" + oldImagePath;
                        if (System.IO.File.Exists(Server.MapPath(oldImageFullPath)))
                        {
                            System.IO.File.Delete(Server.MapPath(oldImageFullPath));
                        }
                        ViewBag.ProcessState = General.Messages.UpdateSuccess;
                        memberBLL = new MemberBLL();
                    }
                }
                else
                {
                    memberBLL.EditMember(model);
                    memberBLL = new MemberBLL();
                    ViewBag.ProcessState = General.Messages.UpdateSuccess;
                }
            }
            model.Statuses = StatusBLL.GetStatusesForDropDown();
            model.ActivityLevels = ActivityLevelBLL.GetActivityLevelsForDropDown();
            if (Session["ID"] != null)
            {
                if ((int)Session["ID"] == model.ID) // Updated yourself
                {
                    Session["Name"] = model.Name;
                    Session["ImagePath"] = model.ImagePath;
                }
            }
            TempData["Success"] = "Update Success";
            return RedirectToAction("Index","Home2");
        }

        public void SendConfirmEmail(string activeCode)
        {
            MailMessage message = new MailMessage();
            //string emailAddress =memberBLL.GetMemberWithEmailAndPassword();
            message.From = new MailAddress("healthhelper666@gmail.com");
            message.To.Add(new MailAddress("kevinwang05010404@gmail.com"));

            //string confirm = $"https://localhost:44327/Home2/Login.cshtml?activecode='{activeCode}'&id='{model.ID}'";

            message.Subject = "HealthHelper 通知";
            message.Body = $"請輸入驗證碼: {activeCode} ,來完成註冊會員的最後一步";

            SmtpClient client = new SmtpClient();
            client.Host = "smtp.gmail.com";
            client.EnableSsl = true;
            NetworkCredential NetworkCred = new NetworkCredential("healthhelper666@gmail.com",
                "hhadmin666");
            client.UseDefaultCredentials = true;
            client.Credentials = NetworkCred;
            client.Port = 25;
            client.Send(message);
            TempData["Sent"] = "會員認證信已寄至信箱";
            //return RedirectToAction("Index");
        }

        public void SendPwdEmail(string password)
        {
            MailMessage message = new MailMessage();          
            message.From = new MailAddress("healthhelper666@gmail.com");
            message.To.Add(new MailAddress("kevinwang05010404@gmail.com"));
           
            message.Subject = "HealthHelper 通知";
            message.Body = $"新密碼: {password} ,請登入後至會員資料更改密碼";

            SmtpClient client = new SmtpClient();
            client.Host = "smtp.gmail.com";
            client.EnableSsl = true;
            NetworkCredential NetworkCred = new NetworkCredential("healthhelper666@gmail.com",
                "hhadmin666");
            client.UseDefaultCredentials = true;
            client.Credentials = NetworkCred;
            client.Port = 25;
            client.Send(message);
            TempData["Sent"] = "新密碼已寄至信箱";          
        }
        public JsonResult IsEmailExist(string email)
        {
            string exist = "";
            if (memberBLL.IsEmailExist(email))
            {
                exist = "true";
            }
            else
            {
                exist = "false";
            }
            return Json(exist, JsonRequestBehavior.AllowGet);
        }
    }
}