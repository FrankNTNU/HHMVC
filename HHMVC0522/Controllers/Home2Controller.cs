using BLL;
using DAL;
using DTO;
using Google.Apis.Auth;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Data.Entity;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace UI.Controllers
{
    public class Home2Controller : Controller
    {
        // GET: Home2
        readonly LayoutBLL layoutBLL = new LayoutBLL();
        readonly PostBLL postBLL = new PostBLL();
        readonly UserBLL userBLL = new UserBLL();
        readonly MemberBLL memberBLL = new MemberBLL();
        public ActionResult Index()
        {
            LayoutDTO layoutDTO = new LayoutDTO();
            layoutDTO = layoutBLL.GetPosts();

            return View(layoutDTO);
        }

        public ActionResult Login(string ReturnUrl)
        {
            ViewBag.ReturnUrl = ReturnUrl;
            UserDTO dto = new UserDTO();
            return View(dto);
        }

        [HttpPost]
        public ActionResult Login(UserDTO model)
        {
            string code = Request.Form["code"].ToString();
            if (model.Email != null && model.Password != null)
            {
                UserDTO user = userBLL.GetUserWithEmailAndPassword(model); 
                if (user.ID != 0&& code == TempData["code"].ToString())
                {

                    Session["ID"] = user.ID;
                    Session["Name"] = user.Name;
                    Session["ImagePath"] = user.ImagePath;
                    Session["UserName"] = user.UserName;
                    Session["Points"] = user.Points;
                    Session["IsAdmin"] = user.IsAdmin;
                    //====================================
                    //恩旗
                    //Use RedirectFromLoginPage to redirect
                    FormsAuthentication.RedirectFromLoginPage(user.ID.ToString(), false);
                    
                    return null;
                    //====================================
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

        public ActionResult LoginFirstTime()
        {
            UserDTO dto = new UserDTO();
            return View(dto);
        }
        [HttpPost]
        public ActionResult LoginFirstTime(UserDTO model)
        {
            if (model.Email != null && model.Password != null && model.ActiveCode!=null)
            {
                UserDTO user = userBLL.GetUserWithEmailAndPwdAndCode(model);
                if (user.ID != 0)
                {
                    Session["ID"] = user.ID;
                    Session["Name"] = user.Name;
                    Session["ImagePath"] = user.ImagePath;
                    Session["Points"] = user.Points;
                    FormsAuthentication.RedirectFromLoginPage(user.ID.ToString(), false);
                    userBLL.ActivateUser(user.ID);
                    return Redirect("~/Home2/Index");
                }
                else
                {
                    ViewBag.ProcessState = General.Messages.LoginFirstError;
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
            Session.Clear();
            FormsAuthentication.SignOut();
            return RedirectToAction("Index");
        }

        public ActionResult PostDetail(int ID)
        {
            //layoutBLL = new LayoutBLL();
            LayoutDTO layoutDTO = new LayoutDTO();
            layoutDTO = layoutBLL.GetPostDetailPageItemWithID(ID);
            return View(layoutDTO);
        }

        [HttpPost]
        [ValidateInput(false)]

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
                    ViewBag.ProcessState = General.Messages.GeneralError;

                }
            }
           
            else
            {
                ViewData["CommentState"] = "Error";
                ViewBag.ProcessState = General.Messages.EmptyArea;

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

        //========================================================
        //恩旗
        //For dialog
        [HttpPost]
        public void SetDialogShownSession() {
            Session["dialogShown"] = true;

            if (Request.Cookies["PointShown"] == null)
            {
                HttpCookie cook = new HttpCookie("PointShown");
                cook.Value = "true";
                cook.Expires = DateTime.Now.AddDays(1).Date;
                Response.Cookies.Add(cook);
            }
        }

        //determine if last 7 days has no weight log
        [NonAction]
        public static void SetWeightLogSession(HttpContextBase context)
        {
            HealthHelperEntities dbContext = new HealthHelperEntities();
            DateTime today = DateTime.Now;
            DateTime d7b = DateTime.Now.Date.AddDays(-7);

            string UserID = context.User.Identity.Name;

            var q1 = dbContext.WeightLogs.Where(wgt => wgt.MemberID.ToString() == UserID
                && wgt.UpdatedDate <= today && wgt.UpdatedDate >= d7b);
            List<WeightLog> list = q1.ToList();
            if (list.Count == 0)
            {
                context.Session["NoWeightLog"] = true;
            }
            else
            {
                context.Session["NoWeightLog"] = false;
            }
        }

        //determine if there is any WorkoutPreferences
        [NonAction]
        public static void SetPreferencesSession(HttpContextBase context)
        {
            HealthHelperEntities dbContext = new HealthHelperEntities();

            string UserID = context.User.Identity.Name;

            var q1 = dbContext.WorkoutPreferences.Where(wp => wp.MemberID.ToString() == UserID);

            if (!q1.Any())
            {
                context.Session["NoPreferences"] = true;
            }
            else
            {
                context.Session["NoPreferences"] = false;
            }
        }

        //determine if user gets points in yesterday's settlement
        public static void SetPointsSession(HttpContextBase context)
        {
            HealthHelperEntities dbContext = new HealthHelperEntities();

            string UserID = context.User.Identity.Name;

            DateTime yesterday = DateTime.Now.Date.AddDays(-1).Date;

            var yesterdayPoints = dbContext.Points.Where(pts => pts.MemberID.ToString() == UserID
                && DbFunctions.TruncateTime(pts.GetPointsDateTime) == yesterday)
                .Select(pts => pts.GetPoints).DefaultIfEmpty(0).Sum();

            context.Session["GotPoints"] = yesterdayPoints;
            
        }

        //determine if a program is frozen, which needs a resultWeight
        public static void SetProgramFrozenSession(HttpContextBase context)
        {
            HealthHelperEntities dbContext = new HealthHelperEntities();

            string UserID = context.User.Identity.Name;

            var program = dbContext.Programs.SingleOrDefault(prg => prg.MemberID.ToString() == UserID
                && prg.StatusID == 3);

            if (program != null)
            {
                int passedDays = (DateTime.Today - program.EndDate.Date).Days < 0 
                    ? 0 : (DateTime.Today - program.EndDate.Date).Days;

                context.Session["ProgramFrozen"] = 7 - passedDays;
            }
            else
            {
                context.Session["ProgramFrozen"] = -1;
            }
        }

        [HttpPost]
        //Set ResultWeight and end the program
        public JsonResult SetResultWeight(int resultWeight) {

            HealthHelperEntities dbContext = new HealthHelperEntities();

            string UserID = User.Identity.Name;
            int MemberID = int.Parse(UserID);

            Program program = dbContext.Programs.SingleOrDefault(prg => prg.MemberID.ToString() == UserID
                && prg.StatusID == 3);

            Session["ProgramFrozen"] = -1;

            decimal weightToLose = program.InitialWeight - program.TargetWeight;
            decimal actuallyLose = program.InitialWeight - resultWeight;

            if (actuallyLose > weightToLose)
            {
                actuallyLose = weightToLose;
            }

            int basePoint = HHDictionary.ProgramSuccessPoint;

            int GetPoints = (int)(basePoint + basePoint * actuallyLose / weightToLose);

            if (actuallyLose > 0)
            {
                dbContext.Points.Add(new DAL.Point
                {
                    MemberID = MemberID,
                    GetPoints = GetPoints,
                    GetPointsDateTime = DateTime.Now,
                    StatusID = 10,
                    ProgramID = program.ID
                });
            }
            else
            {
                dbContext.Points.Add(new DAL.Point
                {
                    MemberID = MemberID,
                    GetPoints = basePoint,
                    GetPointsDateTime = DateTime.Now,
                    StatusID = 10,
                    ProgramID = program.ID
                });
            }

            program.ResultWeight = resultWeight;
            program.StatusID = 5;

            //write resulWeight to WeightLog incidentally
            WeightLog weightLog = dbContext.WeightLogs
                .SingleOrDefault(wgtl => wgtl.MemberID.ToString() == UserID 
                && DbFunctions.TruncateTime(wgtl.UpdatedDate) == DateTime.Today);

            if (weightLog != null)
            {
                weightLog.Weight = resultWeight;
                weightLog.UpdatedDate = DateTime.Now;
            }
            else
            {
                dbContext.WeightLogs.Add(new WeightLog
                {
                    Weight = resultWeight,
                    MemberID = MemberID,
                    UpdatedDate = DateTime.Now
                });
            }

            dbContext.SaveChanges();

            //set Session["NoWeightLog"] to false
            Session["NoWeightLog"] = false;

            return Json(new { Result = "Success", GetPoints = GetPoints });
            
        }

        //determine if user is in a program
        [NonAction]
        public static void SetInProgramSession(HttpContextBase context)
        {
            HealthHelperEntities dbContext = new HealthHelperEntities();

            string UserID = context.User.Identity.Name;

            var program = dbContext.Programs.SingleOrDefault(prg => prg.MemberID.ToString() == UserID
                && DbFunctions.TruncateTime(prg.StartDate) <= DateTime.Today
                && DbFunctions.TruncateTime(prg.EndDate) >= DateTime.Today
                && prg.StatusID == 1);

            if (program != null)
            {
                context.Session["InProgram"] = true;
            }
            else
            {
                context.Session["InProgram"] = false;
            }
        }

        public JsonResult GetProgramWeight()
        {
            HealthHelperEntities dbContext = new HealthHelperEntities();

            Program program = dbContext.Programs
                .SingleOrDefault(prg => prg.MemberID.ToString() == User.Identity.Name
                && prg.StatusID == 3);

            return Json(new { initWeight = program.InitialWeight, tgtWeight = program.TargetWeight });
        }
        //=========================================================

        public ActionResult UserPoints()
        {
            if (Session["ID"] == null)
                return Redirect("~/Home2/Login");
            Session["Points"] = userBLL.GetPoints((int)Session["ID"]);
            return View();
        }
        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        public ActionResult GoogleLogin()
        {

            UserDTO dto = new UserDTO();
            return View(dto);
        }

        [HttpPost]
        public async Task<ActionResult> GoogleLogin(string id_token)
        {
            string msg = "ok";
            GoogleJsonWebSignature.Payload payload = null;
            try
            {
                payload = await GoogleJsonWebSignature.ValidateAsync(id_token, new GoogleJsonWebSignature.ValidationSettings()
                {
                    Audience = new List<string>() { "210856283403-257sicfc1u5afrca31mtevcc975u1dk5.apps.googleusercontent.com" }//要驗證的client id，把自己申請的Client ID填進去
                });
            }
            catch (Google.Apis.Auth.InvalidJwtException ex)
            {
                msg = ex.Message;
            }
            catch (Newtonsoft.Json.JsonReaderException ex)
            {
                msg = ex.Message;
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }
            string user_id = null;
            if (msg == "ok" && payload != null)
            {//都成功
                user_id = payload.Subject;//取得user_id
                msg = $@"您的 user_id :{user_id}";
            }

            if (user_id != null)
            {
                UserDTO user = userBLL.GetUserWithGoogleID(user_id);
                if (user.ID != 0)
                {
                    Session["ID"] = user.ID;
                    Session["Name"] = user.Name;
                    Session["ImagePath"] = user.ImagePath;
                    Session["Points"] = user.Points;
                    FormsAuthentication.RedirectFromLoginPage(user.ID.ToString(), false);
                    //return Redirect("/Home2/Index");
                    return null;
                }
                else
                {
                    ViewBag.ProcessState = General.Messages.LoginError;
                    return RedirectToAction("Login", "Home2");
                }
            }
            else
            {
                ViewBag.ProcessState = General.Messages.GoogleLoginError;
                return RedirectToAction("Login", "Home2");
            }
        }
        public JsonResult AddGoogleID(string email, string googleID)
        {
            memberBLL.AddGoogleID(email, googleID);
            return Json("");
        }
        public ActionResult GetNewPassword(string email)
        {
            string password = Guid.NewGuid().ToString().Substring(0, 6);
            memberBLL.GetNewPassword(email, password);
            MemberController memberController = new MemberController();
            memberController.SendPwdEmail(password);
            return RedirectToAction("Login");
        }

        private string RandomCode(int length)
        {
            string s = "0123456789zxcvbnmasdfghjklqwertyuiop";
            StringBuilder sb = new StringBuilder();
            Random rand = new Random();
            int index;
            for (int i = 0; i < length; i++)
            {
                index = rand.Next(0, s.Length);
                sb.Append(s[index]);
            }
            return sb.ToString();
        }

        private void PaintInterLine(Graphics g, int num, int width, int height)
        {
            Random r = new Random();
            int startX, startY, endX, endY;
            for (int i = 0; i < num; i++)
            {
                startX = r.Next(0, width);
                startY = r.Next(0, height);
                endX = r.Next(0, width);
                endY = r.Next(0, height);
                g.DrawLine(new Pen(Brushes.Red), startX, startY, endX, endY);
            }
        }

        public ActionResult GetValidateCode()
        {
            byte[] data = null;
            string code = RandomCode(5);
            TempData["code"] = code;
            //定義一個畫板
            MemoryStream ms = new MemoryStream();
            using (Bitmap map = new Bitmap(100, 40))
            {
                //畫筆,在指定畫板畫板上畫圖
                //g.Dispose();
                using (Graphics g = Graphics.FromImage(map))
                {
                    g.Clear(Color.White);
                    g.DrawString(code, new Font("黑體", 22.0F), Brushes.Blue, new System.Drawing.Point(10, 8));
                    //繪製干擾線(數字代表幾條)
                    PaintInterLine(g, 10, map.Width, map.Height);
                }
                map.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
            }
            data = ms.GetBuffer();
            return File(data, "image/jpeg");
        }

        [HttpPost]
        public JsonResult ChangePwd(string newPassword)
        {
            using (HealthHelperEntities db = new HealthHelperEntities())
            {
                int userID = (int)Session["ID"];
                Member member = db.Members.FirstOrDefault(x => x.ID == userID);
                member.Password = newPassword;
                db.SaveChanges();
            }           
            return Json("");
        }
       [HttpPost]
        public JsonResult CheckOldPwd(string oldPassword)
        {
            bool theSame = false;
            using (HealthHelperEntities db = new HealthHelperEntities())
            {
                int userID = (int)Session["ID"];
                Member member = db.Members.FirstOrDefault(x => x.ID ==userID );
                theSame = oldPassword == member.Password;
            }
            return Json(theSame);
        }
    }
}