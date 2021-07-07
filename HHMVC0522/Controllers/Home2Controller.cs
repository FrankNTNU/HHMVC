using BLL;
using DTO;
using Google.Apis.Auth;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
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
        LayoutBLL layoutBLL = new LayoutBLL();
        PostBLL postBLL = new PostBLL();
        UserBLL userBLL = new UserBLL();
        MemberBLL memberBLL = new MemberBLL();
        public ActionResult Index()
        {
            LayoutDTO layoutDTO = new LayoutDTO();
            layoutDTO = layoutBLL.GetPosts();
            return View(layoutDTO);
        }
        public ActionResult Login()
        {
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

                    FormsAuthentication.RedirectFromLoginPage(user.ID.ToString(), false);
                    return RedirectToAction("Index", "Home2");
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
                    FormsAuthentication.RedirectFromLoginPage(user.ID.ToString(), false);
                    userBLL.ActivateUser(user.ID);
                    return RedirectToAction("Index", "Home2");
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
            string user_id=null;
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
            string password=Guid.NewGuid().ToString().Substring(0, 6);
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
                    g.DrawString(code, new Font("黑體", 18.0F), Brushes.Blue, new Point(10, 8));
                    //繪製干擾線(數字代表幾條)
                    PaintInterLine(g, 10, map.Width, map.Height);
                }
                map.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
            }
            data = ms.GetBuffer();
            return File(data, "image/jpeg");
        }

    }
}