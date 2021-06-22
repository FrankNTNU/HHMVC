using BLL;
using DTO;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace UI.Controllers
{
    public class FrontGiftController : Controller
    {
        // GET: FrontGift
        public ActionResult Index()
        {
            return View();
        }
        GiftBLL giftBLL = new GiftBLL();
        public ActionResult GiftList()
        {
            UserBLL userBLL = new UserBLL();
            if (Session["ID"] != null)
            {
                Session["Points"] = userBLL.GetPoints((int)Session["ID"]);
            }
            List<GiftDTO> giftDTOs = new List<GiftDTO>();
            giftDTOs = giftBLL.GetGifts();
            return View(giftDTOs);
        }
        static int giftID;
        public ActionResult GiftDetail(int ID)
        {
            GiftDTO model = new GiftDTO();
            model = giftBLL.GetGift(ID);
            giftID = ID;
            return View(model);
        }
        GiftCartBLL cartBLL = new GiftCartBLL();
        public ActionResult GiftCart(int userID)
        {
            List<GiftCartDTO> carts = new List<GiftCartDTO>();
            carts = cartBLL.GetGiftCarts(userID);
            return View(carts);
        }
        public JsonResult IsSameItemExist(int giftID) 
        {
            bool isExist = cartBLL.IsSameItemExist((int)Session["ID"], giftID);
            string isItemExist = isExist ? "yes" : "no";
            return Json(isItemExist, JsonRequestBehavior.AllowGet);
        }
        public ActionResult AddCart(int giftID)
        {
            GiftDTO giftDTO = new GiftDTO();
            giftDTO = giftBLL.GetGift(giftID);
            string fileName = giftDTO.Image;
            string sourcePath = @"~/Areas/Admin/Content/GiftImages/";
            string targetPath = @"~/Areas/Admin/Content/CartImages/";
            string sourceFile = Path.Combine(Server.MapPath(sourcePath), fileName);
            string destFile = Path.Combine(Server.MapPath(targetPath), fileName);
            System.IO.File.Copy(sourceFile, destFile, true);
            if (cartBLL.AddCart(giftDTO))
            {
                UserBLL userBLL = new UserBLL();
                Session["Points"] = userBLL.GetPoints((int)Session["ID"]);
                giftBLL.RemoveOneGift(giftID);
                
                return RedirectToAction("GiftCart", new { userID = (int)Session["ID"] } );
            }
            else
            {
                return RedirectToAction("GiftDetail", new { ID = giftID });
            }
        }
        public JsonResult GetGifts(string text)
        {
            List<GiftDTO> dtoList = giftBLL.GetGifts(text);
            return Json(dtoList, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetOrderedGifts(string isAscending)
        {
            bool isAscend = isAscending == "0" ? true : false;
            List<GiftDTO> dtoList = giftBLL.GetGifts(isAscend);
            return Json(dtoList, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetCarts(string text)
        {
            List<GiftCartDTO> dtoList = cartBLL.GetGiftCarts(text);
            return Json(dtoList, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetOrderedCarts(string isAscending)
        {
            bool isAscend = isAscending == "1" ? true : false;
            List<GiftCartDTO> dtoList = cartBLL.GetGiftCarts(isAscend);
            return Json(dtoList, JsonRequestBehavior.AllowGet);
        }
        public ActionResult SendBarcode(string barcode)
        {
            // 建立字體
            PrivateFontCollection fontCollection = new PrivateFontCollection();
            fontCollection.AddFontFile(Server.MapPath("~/Areas/Admin/Content/webfonts/BarcodeFont.ttf"));
            FontFamily family = new FontFamily("barcode font", fontCollection);
            Font barcodeFont = new Font(family, 128);

            // 繪製條碼
            Bitmap tmpBitmap = new Bitmap(550, 220, PixelFormat.Format32bppArgb);
            Graphics graphics = Graphics.FromImage(tmpBitmap);
            graphics.DrawString(barcode, barcodeFont, new SolidBrush(Color.Black), 0, 0);

            // 儲存圖片
            var imagePath = Server.MapPath("~/Areas/Admin/Content/Barcode/" + barcode + ".png");
            if (!System.IO.File.Exists(imagePath))
                tmpBitmap.Save(imagePath, ImageFormat.Png);

            // 設定郵件位址
            MailMessage message = new MailMessage();
            message.From = new MailAddress("healthhelper666@gmail.com");
            message.To.Add(new MailAddress("healthhelper666@gmail.com"));

            // 附件圖片
            var barcodeImage = Server.MapPath("~/Areas/Admin/Content/Barcode/" + barcode + ".png");
            var stream = new WebClient().OpenRead(barcodeImage);
            string fileName = Path.GetFileName(barcodeImage);
            message.Attachments.Add(new Attachment(stream, fileName));
            message.Subject = "HealthHelper 通知";
            message.Body = Session["Name"] + "，您的禮物兌換條碼已送達！";
            SmtpClient client = new SmtpClient();
            client.Host = "smtp.gmail.com";
            client.EnableSsl = true;
            NetworkCredential NetworkCred = new NetworkCredential("healthhelper666@gmail.com",
                "hhadmin666");
            client.UseDefaultCredentials = true;
            client.Credentials = NetworkCred;
            client.Port = 25;
            client.Send(message);
            TempData["Sent"] = "Success";
            return RedirectToAction("GiftCart", new { userID = (int)Session["ID"] }) ;
        }
    }
}