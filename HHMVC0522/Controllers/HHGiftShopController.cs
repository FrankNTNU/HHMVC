using BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace UI.Controllers
{
    public class HHGiftShopController : Controller
    {
        // GET: HHGiftShop
        MemberBLL mBLL = new MemberBLL();
        GiftBLL gBLL = new GiftBLL();
        //public ActionResult MyGiftShop()
        //{
            
        //    return View(gBLL.GetGiftByLevel());
        //}
    }
}