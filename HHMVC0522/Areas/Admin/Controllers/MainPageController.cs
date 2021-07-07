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
            // 寫一個取得會員總人數的方法
            // 取得方法之後將數字丟給頁面

            // 1. 定義一個工廠(BLL)實體
            CountBLL countBLL = new CountBLL();
            // 2. 定義一個CountDTO實體
            CountDTO countDTO = new CountDTO();
            // 3. 設定countDTO裡面的屬性
            countDTO = countBLL.GetCounts();
            // 4. 把countDTO丟給View頁面
            return View(countDTO);
        }
    }
}