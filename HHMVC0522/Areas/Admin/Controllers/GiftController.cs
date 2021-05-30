using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BLL;
using DTO;

namespace UI.Areas.Admin.Controllers
{
    public class GiftController : Controller
    {
        // GET: Admin/Gift
        public ActionResult List()
        {
            GiftBLL bll = new GiftBLL();
            return View(bll.GetGift());
        }
        public ActionResult Creat()
        {
            GiftDTO gift = new GiftDTO();
            return View(gift);
        }
        [HttpPost]
        public ActionResult Creat(GiftDTO gift)
        {
            Bitmap image = new Bitmap(gift.UpLoadImage.InputStream);
            Bitmap resizedImage = new Bitmap(image, 500, 500);
            string uniqueNumber = Guid.NewGuid().ToString();
            string fileName = uniqueNumber + gift.UpLoadImage.FileName;
            resizedImage.Save(Server.MapPath("~/Areas/Admin/Content/GiftImages/" + fileName));
            gift.Image = fileName;
            GiftBLL bll = new GiftBLL();
            bll.Add(gift);
            return RedirectToAction("List");
        }
    }
}