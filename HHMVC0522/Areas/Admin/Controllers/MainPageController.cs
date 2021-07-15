using BLL;
using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace UI.Areas.Admin.Controllers
{
    public class MainPageController : BaseController
    {
        // GET: Admin/MainPage
        public ActionResult Index()
        {
            return View();
        }
        // 資料傳遞方向 (前端到後端)
        // View 前端頁面 > Action 動作 > BLL 工廠 > DAL 資料存取層 > DB 資料庫
        // 資料內容型式 (DTO資料傳遞物件: 很多屬性)
        // 
        public ActionResult MainPage() // 主畫面的動作
        {
            CountBLL countBLL = new CountBLL();
            CountDTO countDTO = countBLL.GetCounts();
            return View(countDTO);
        }
    }
}