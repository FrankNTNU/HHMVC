using BLL;
using DTO;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace UI.Areas.Admin.Controllers
{
    public class GiftController : BaseController
    {
        // GET: Admin/Gift
        public ActionResult Index()
        {
            return View();
        }
        GiftBLL giftBLL = new GiftBLL();
        public ActionResult GiftList()
        {
            List<GiftDTO> giftList = new List<GiftDTO>();
            giftList = giftBLL.GetGifts();
            return View(giftList);
        }
        public ActionResult AddGift()
        {
            GiftDTO giftDTO = new GiftDTO();
            return View(giftDTO);
        }
        [HttpPost]
        public ActionResult AddGift(GiftDTO model)
        {
            //giftBLL.AddGift(model);
            if (model.UploadImage == null)
            {
                ViewBag.ProcessState = General.Messages.ImageMissing;
            }
            else if (ModelState.IsValid)
            {
                string ext = Path.GetExtension(model.UploadImage.FileName);
                if (ext != ".png" && ext != ".jpg" && ext != ".jpeg")
                {
                    ViewBag.ProcessState = General.Messages.ExtensionError;
                    return View(model);
                }
                Bitmap image = new Bitmap(model.UploadImage.InputStream); // 把上傳圖片轉成Bitmap
                Bitmap resizedImage = new Bitmap(image, 500, 500); // 設定長寬
                string uniqueNumber = Guid.NewGuid().ToString(); // 設定唯一字串
                string fileName = uniqueNumber + model.UploadImage.FileName; // 圖片路徑  = 唯一字串 + 圖片檔名
                
                model.Image = fileName; // 把圖片路徑存在DTO的屬性
                
                if (giftBLL.AddGift(model))
                {
                    ViewBag.ProcessState = General.Messages.AddSuccess;
                    ModelState.Clear();
                    resizedImage.Save(Server.MapPath("~/Areas/Admin/Content/GiftImages/" + fileName)); // 存在資料夾
                    model = new GiftDTO();
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
            return View(model);
        }
        public ActionResult UpdateGift(int ID)
        {
            GiftDTO model = giftBLL.GetGift(ID);
            model.IsUpdate = true;
            model.EndDate = model.EndDate.Date;
            return View(model);
        }
        [HttpPost]
        public ActionResult UpdateGift(GiftDTO model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.ProcessState = General.Messages.EmptyArea;
            }
            else
            {
                if (model.UploadImage != null) // Image has been changed.
                {
                    HttpPostedFileBase postedFile = model.UploadImage;
                    string ext = Path.GetExtension(postedFile.FileName);
                    if (ext == ".jpg" || ext == ".jpeg" || ext == ".png" || ext == ".gif")
                    {
                        Bitmap userImage = new Bitmap(postedFile.InputStream);
                        if (Math.Abs(((double)userImage.Height / userImage.Width) - 1) > 0.25)
                        {
                            ViewBag.ProcessState = General.Messages.WrongImageSize;
                            
                            return View(model);
                        }
                        Bitmap resizedImage = new Bitmap(userImage, 500, 500);
                        string uniqueNumber = Guid.NewGuid().ToString();
                        string fileName = uniqueNumber + postedFile.FileName;
                        resizedImage.Save(Server.MapPath("~/Areas/Admin/Content/GiftImages/" + fileName));
                        model.Image = fileName;
                    }
                    string oldImagePath = giftBLL.UpdateGift(model);
                    if (model.UploadImage != null)
                    {
                        string oldImageFullPath = "~/Areas/Admin/Content/GiftImages/" + oldImagePath;
                        if (System.IO.File.Exists(Server.MapPath(oldImageFullPath)))
                        {
                            System.IO.File.Delete(Server.MapPath(oldImageFullPath));
                        }
                        ViewBag.ProcessState = General.Messages.UpdateSuccess;
                        giftBLL = new GiftBLL();
                    }
                }
                else
                {
                    giftBLL.UpdateGift(model);
                    giftBLL = new GiftBLL();
                    ViewBag.ProcessState = General.Messages.UpdateSuccess;
                }
            }
            return View(model);
        }
        public JsonResult DeleteGift(int ID)
        {
            string imagePath = giftBLL.DeleteGift(ID);
            string ImageFullPath = Server.MapPath(@"~\Areas\Admin\Content\GiftImages\" + imagePath);
            if (System.IO.File.Exists(ImageFullPath))
            {
                System.IO.File.Delete(ImageFullPath);
            }
            giftBLL = new GiftBLL();
            return Json("");
        }
        public JsonResult DeleteGiftCart(int ID)
        {
            string imagePath = giftCartBLL.DeleteCart(ID);
            string ImageFullPath = Server.MapPath(@"~\Areas\Admin\Content\CartImages\" + imagePath);
            if (System.IO.File.Exists(ImageFullPath))
            {
                System.IO.File.Delete(ImageFullPath);
            }
            giftCartBLL = new GiftCartBLL();
            return Json("");
        }
        GiftCartBLL giftCartBLL = new GiftCartBLL();

        public ActionResult GiftCarts()
        {
            List<GiftCartDTO> modelList = giftCartBLL.GetGiftCarts();
            return View(modelList);
        }
    }
}