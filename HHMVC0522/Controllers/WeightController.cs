using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace UI.Controllers
{
    public class WeightController : Controller
    {
        HealthHelperEntities db = new HealthHelperEntities();

        // GET: Weight
        public ActionResult WeightLog()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult GetWeightLog()
        {
            var q = db.WeightLogs.Where(wgt => wgt.MemberID == 83).Select(wgt => new 
            { 
                wgt.ID,
                wgt.UpdatedDate,
                wgt.Weight
            });

            return Json(q.ToList());
        }


        public JsonResult GetWeightLogForChart()
        {
            var q = db.WeightLogs.Where(wgt => wgt.MemberID == 83).Select(wgt => wgt.Weight);

            return Json(q.ToList());
        }


    }
}