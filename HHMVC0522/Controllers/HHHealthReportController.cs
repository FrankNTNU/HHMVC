using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UI.ViewModels;

namespace UI.Controllers
{
    public class HHHealthReportController : Controller
    {
        // GET: HealthReport
        public ActionResult HealthReport()
        {

            int memberID = 17; 
            if (Session["ID"] != null)
            {
                memberID = (int)Session["ID"];
            }
            HealthReportChartViewModel model = new HealthReportChartViewModel(memberID, DateTime.Today);
            return View(model);
        }
    }
}