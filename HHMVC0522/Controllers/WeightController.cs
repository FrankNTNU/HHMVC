using DAL;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace UI.Controllers
{
    public class WeightController : Controller
    {
        HealthHelperEntities db = new HealthHelperEntities();

        // GET: Weight
        public ActionResult WeightLog()
        {
            DateTime startMonth = new DateTime(DateTime.Now.Year, 1, 1);
            DateTime endMonth = DateTime.Now;

            var q = from wgt in db.WeightLogs.Where(wgt => wgt.MemberID == 83).ToList()
                    where startMonth <= wgt.UpdatedDate && wgt.UpdatedDate < endMonth
                    group wgt by wgt.UpdatedDate.ToString("yyMM") into g
                    orderby g.Key ascending
                    select g.Average(wgt => wgt.Weight);

            ViewBag.initialWeightLog = Newtonsoft.Json.JsonConvert.SerializeObject(q.ToList());

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


        public JsonResult GetWeightLogForChart(DateTime startMonth, DateTime endMonth)
        {
            //var q = db.WeightLogs.Where(wgt => wgt.MemberID == 83).Select(wgt => wgt.Weight);

            endMonth = endMonth.AddMonths(1);

            var q = from wgt in db.WeightLogs.Where(wgt => wgt.MemberID == 83).ToList()
                    where startMonth <= wgt.UpdatedDate && wgt.UpdatedDate < endMonth
                    group wgt by wgt.UpdatedDate.ToString("yyMM") into g
                    orderby g.Key ascending
                    select g.Average(wgt => wgt.Weight);

            return Json(q.ToList());

            //return Json(new { start = startMonth, end = endMonth });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddWeightLog(WeightLog wgtl)
        {
            DateTime today = DateTime.Now.Date;

            if (db.WeightLogs
                .Any(wgtl1 => DbFunctions.TruncateTime(wgtl1.UpdatedDate) == today))
            {
                return Json(new { Result = "failed", Error = "今天已新增紀錄過了" });
            }

            wgtl.MemberID = 83;
            wgtl.UpdatedDate = DateTime.Now;

            db.WeightLogs.Add(wgtl);

            try
            {
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                return Json(new { Result = "failed", Error = ex.Message });
            }

            return Json(new { Result = "success", Error = "none", ID = wgtl.ID});
        }

        public JsonResult EditWeightLog(WeightLog wgtlToEdit)
        {
            WeightLog wgtl = db.WeightLogs
                .SingleOrDefault(wgtl1 => wgtl1.ID == wgtlToEdit.ID 
                && wgtl1.MemberID == 83);

            wgtl.Weight = wgtlToEdit.Weight;

            try
            {
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                return Json(new { Result = "failed", Error = ex.Message });
            }

            return Json(new { Result = "success", Error = "none"});
        }

        //======================================================
        //Session Test

        //public JsonResult SessionTest()
        //{
        //    int count = 0;
        //    if (Session["count"] != null)
        //    {
        //        count = (int)Session["count"];
        //    }
        //    count++;
        //    Session["count"] = count;

        //    return Json(new { count = (int)Session["count"] });
        //}
    }
}