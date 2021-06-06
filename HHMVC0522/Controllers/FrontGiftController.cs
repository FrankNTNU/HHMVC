using BLL;
using DTO;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
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
            UserStatic.Points = userBLL.GetPoints(UserStatic.UserID);
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
            bool isExist = cartBLL.IsSameItemExist(UserStatic.UserID, giftID);
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
                UserStatic.Points = userBLL.GetPoints(UserStatic.UserID);
                giftBLL.RemoveOneGift(giftID);
                
                return RedirectToAction("GiftCart", new { userID = UserStatic.UserID } );
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
    }
}