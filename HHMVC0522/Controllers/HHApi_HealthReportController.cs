using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UI.ViewModels;

namespace UI.Controllers
{
    public class HHApi_HealthReportController : Controller
    {
        // GET: HHApi_HealthReport


            public JsonResult GetGeneralHealthChartDatas(int memberId, int monthFlag)
        {
            HealthReportChartViewModel model = new HealthReportChartViewModel(memberId, DateTime.Today.AddMonths(monthFlag));

            return Json(model, JsonRequestBehavior.AllowGet);

        }
    }
}
