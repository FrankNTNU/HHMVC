using BLL;
using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UI.ViewModels;

namespace UI.Controllers
{
    public class HHApi_WaterLogController : Controller
    {
        // GET: HHApi_WaterLog
        

        WaterLogBLL wBLL = new WaterLogBLL();
        public JsonResult SendWaterLog(WaterLog waterLogAmountToAdded)
        {
            wBLL.UpdateWaterLog(waterLogAmountToAdded);
            int[] currentWeekWaterLogs = wBLL.GetCurrentWeekWaterLogs(waterLogAmountToAdded.MemberID);

            return Json(currentWeekWaterLogs, JsonRequestBehavior.AllowGet);

        }

        public JsonResult GetWeeklyWateLogChartDatas(int memberId, string date)   //all data
        {
            WaterLogDatasViewModel model = new WaterLogDatasViewModel(memberId, date);
            return Json(model, JsonRequestBehavior.AllowGet);

        }


    }
}