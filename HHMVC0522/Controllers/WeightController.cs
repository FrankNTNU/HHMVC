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

    [Authorize]
    public class WeightController : Controller
    {
        HealthHelperEntities db = new HealthHelperEntities();

        // GET: Weight
        public ActionResult WeightLog()
        {
            DateTime startMonth = new DateTime(DateTime.Now.Year, 1, 1);
            DateTime endMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month + 1, 1, 0, 0, 0);

            int MemberID = (int)Session["ID"];

            var q = from wgt in db.WeightLogs.Where(wgt => wgt.MemberID == MemberID).ToList()
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
            int MemberID = (int)Session["ID"];

            var q = db.WeightLogs.Where(wgt => wgt.MemberID == MemberID).Select(wgt => new 
            { 
                wgt.ID,
                wgt.UpdatedDate,
                wgt.Weight
            });

            return Json(q.ToList());
        }


        public JsonResult GetWeightLogForChart(DateTime startMonth, DateTime endMonth)
        {
            //var q = db.WeightLogs.Where(wgt => wgt.MemberID == (int)Session["ID"]).Select(wgt => wgt.Weight);

            endMonth = endMonth.AddMonths(1);

            int MemberID = (int)Session["ID"];

            var q1 = from wgt in db.WeightLogs.Where(wgt => wgt.MemberID == MemberID).ToList()
                     where startMonth <= wgt.UpdatedDate && wgt.UpdatedDate < endMonth
                     group wgt by int.Parse(wgt.UpdatedDate.ToString("yyMM")) into g
                     orderby g.Key ascending
                     select new KeyValuePair<int, double>(g.Key, g.Average(wgt => wgt.Weight));

            Dictionary<int, double> monthDict = q1.ToDictionary(g => g.Key, g => g.Value);
            
            int smonth = int.Parse(startMonth.ToString("yyMM"));
            int emonth = int.Parse(endMonth.ToString("yyMM"));
            for (; smonth < emonth; smonth++)
            {
                if (smonth % 100 > 12) 
                {
                    smonth = smonth / 100 * 100 + 101;
                }

                if (smonth != emonth && !monthDict.Keys.Contains(smonth))
                {
                    monthDict.Add(smonth, 0d);
                }
            }

            return Json(monthDict.OrderBy(d => d.Key).Select(d => d.Value).ToList());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddWeightLog(WeightLog wgtl)
        {
            DateTime today = DateTime.Now.Date;
            int MemberID = (int)Session["ID"];

            if (db.WeightLogs
                .Any(wgtl1 => DbFunctions.TruncateTime(wgtl1.UpdatedDate) == today
                && wgtl1.MemberID == MemberID))
            {
                return Json(new { Result = "failed", Error = "今天已新增紀錄過了" });
            }

            wgtl.MemberID = MemberID;
            wgtl.UpdatedDate = DateTime.Now;

            db.WeightLogs.Add(wgtl);

            try
            {
                db.SaveChanges();
                Session["NoWeightLog"] = false;
            }
            catch (Exception ex)
            {
                
                return Json(new { Result = "failed", Error = ex.Message });
            }

            return Json(new { Result = "success", Error = "none", ID = wgtl.ID});
        }

        public JsonResult EditWeightLog(WeightLog wgtlToEdit)
        {
            int MemberID = (int)Session["ID"];

            WeightLog wgtl = db.WeightLogs
                .SingleOrDefault(wgtl1 => wgtl1.ID == wgtlToEdit.ID 
                && wgtl1.MemberID == MemberID);

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