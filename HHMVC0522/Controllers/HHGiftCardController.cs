using BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace UI.Controllers
{
    public class HHGiftCardController : Controller
    {
        // GET: HHGiftCard
        MemberBLL mBLL = new MemberBLL();
        public ActionResult ShowGiftList()
        {
            return View(mBLL.GetMemberByMemberID(17));
        }
    }
}