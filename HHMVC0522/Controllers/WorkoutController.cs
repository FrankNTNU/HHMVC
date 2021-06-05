using DAL;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;

namespace UI.Controllers
{

    public class WorkoutController : Controller
    {
        HealthHelperEntities dbContext = new HealthHelperEntities();

        //==========================================================
        //WorkoutLog Page

        public ActionResult WorkoutLog()
        {
            return View(dbContext.Workouts.ToList());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetWorkoutLog()
        {
            DateTime today = DateTime.Now.Date;

            DateTime tomorrow = today.AddDays(1);

            var q1 = dbContext.WeightLogs
                .Where(wgt => wgt.MemberID == 83 && wgt.UpdatedDate < tomorrow)
                .OrderByDescending(wgt => wgt.UpdatedDate)
                .FirstOrDefault();

            decimal weight = (decimal)q1.Weight;

            var q = dbContext.WorkoutLogs.Where(wl => wl.MemberID == 83).ToList()
                .Select(wl => new
                {
                    wl.ID,
                    wl.WorkoutID,
                    wl.EditTime,
                    wl.Workout.Name,
                    wl.WorkoutTime,
                    wl.WorkoutHours,
                    Consume = (wl.Workout.Calories * wl.WorkoutHours * (double)weight).ToString("0.00"),
                    Status = wl.Status.Name
                });

            return Json(q.ToList());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult GetWorkoutAlWc(int wid)
        {
            var workout = dbContext.Workouts.SingleOrDefault(w => w.ID == wid);

            return Json(new { Al = workout.ActivityLevel.Description, Wc = workout.WorkoutCategory.Name });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddWorkoutLog(WorkoutLog wl)
        {

            wl.MemberID = 83;
            wl.EditTime = DateTime.Now;

            dbContext.WorkoutLogs.Add(wl);

            try
            {
                dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                return Json(new { Result = "failed", Error = ex.Message });
            }

            return Json(new { Result = "success", Error = "none", ID = wl.ID });

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult DeleteWorkoutLog(int ID)
        {

            WorkoutLog workoutlog = dbContext.WorkoutLogs.SingleOrDefault(wl => wl.ID == ID && wl.MemberID == 83);
            dbContext.WorkoutLogs.Remove(workoutlog);

            try
            {
                dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                return Json(new { Result = "failed", Error = ex.Message });
            }

            return Json(new { Result = "success", Error = "none" });

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult EditWorkoutLog(WorkoutLog wlToEdit)
        {

            WorkoutLog workoutlog = dbContext.WorkoutLogs.SingleOrDefault(wl => wl.ID == wlToEdit.ID);
            workoutlog.MemberID = 83;
            workoutlog.EditTime = DateTime.Now;

            workoutlog.WorkoutID = wlToEdit.WorkoutID;
            workoutlog.WorkoutTime = wlToEdit.WorkoutTime;
            workoutlog.WorkoutHours = wlToEdit.WorkoutHours;
            workoutlog.StatusID = wlToEdit.StatusID;

            try
            {
                dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                return Json(new { Result = "failed", Error = ex.Message });
            }

            return Json(new { Result = "success", Error = "none" });

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult GetEstimateCal(int wid, double whours)
        {
            return Json(new { Consume = EstimateCal(wid, whours) });
        }

        [NonAction]
        public string EstimateCal(int wid, double whours)
        {
            DateTime today = DateTime.Now.Date;
            DateTime tomorrow = today.AddDays(1);

            var cal = dbContext.Workouts.SingleOrDefault(w => w.ID == wid).Calories;
            var weight = GetCurrentWeight(tomorrow);

            return (cal * whours * (double)weight).ToString("0.00");
        }

        //======================================================
        //WorkoutSchedule Page

        //todo
        public ActionResult WorkoutSchedule()
        {
            decimal TDEE = 2000;
            decimal todayIngest = TodayIngest();
            decimal todayConsume = TodayConsume();

            TodayCaloriesToPercent(ref TDEE, ref todayIngest, ref todayConsume);

            ViewBag.TDEE = TDEE;
            ViewBag.Ingest = todayIngest;
            ViewBag.Consume = todayConsume;

            return View();
        }

        [NonAction]
        private void TodayCaloriesToPercent(ref decimal TDEE, ref decimal todayIngest, ref decimal todayConsume)
        {
            decimal tempTDEE = TDEE;
            decimal tempIngest = todayIngest;
            decimal tempConsume = todayConsume;


            if (tempTDEE >= tempIngest && tempTDEE >= tempConsume)
            {
                TDEE = 100;
                todayIngest = Math.Round(tempIngest / tempTDEE * 100, 1);
                todayConsume = Math.Round(tempConsume / tempTDEE * 100, 1);
            }
            else if (tempIngest >= tempTDEE && tempIngest >= tempConsume)
            {
                todayIngest = 100;
                todayConsume = Math.Round(tempConsume / tempIngest * 100, 1);
                TDEE = Math.Round(tempTDEE / tempIngest * 100, 1);
            }
            else
            {
                todayConsume = 100;
                TDEE = Math.Round(tempTDEE / tempConsume * 100, 1);
                todayIngest = Math.Round(tempIngest / tempConsume * 100, 1);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult GetSchedule()
        {
            DateTime tomorrow = DateTime.Now.AddDays(1).Date;
            DateTime d8a = DateTime.Now.AddDays(8).Date;

            var q = dbContext.WorkoutLogs.Where(wl => wl.MemberID == 83
                && DbFunctions.TruncateTime(wl.WorkoutTime) >= tomorrow
                && DbFunctions.TruncateTime(wl.WorkoutTime) < d8a && wl.StatusID == 4)
                .Select(wl => new
                {
                    wl.ID,
                    wl.WorkoutTime,
                    wl.Workout.Name,
                    wl.WorkoutHours
                }).OrderBy(wl => wl.WorkoutTime);

            return Json(q.ToList());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult CheckTodaySchedule(int ID)
        {

            var workoutLog = dbContext.WorkoutLogs.SingleOrDefault(wl => wl.ID == ID);

            workoutLog.StatusID = 5;

            try
            {
                dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                return Json(new { Result = "failed", Error = ex.Message });
            }

            decimal TDEEPercent = 2000;
            decimal IngestPercent = TodayIngest();
            decimal ConsumePercent = TodayConsume();
            decimal todayConsume = ConsumePercent;

            TodayCaloriesToPercent(ref TDEEPercent, ref IngestPercent, ref ConsumePercent);

            return Json(new 
            { 
                Result = "success", 
                Error = "none", 
                TDEEPercent = TDEEPercent,
                IngestPercent = IngestPercent,
                ConsumePercent = ConsumePercent,
                TodayConsume = todayConsume
            });

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult GetTodayWorkout()
        {

            DateTime d = DateTime.Now.Date;

            var q = dbContext.WorkoutLogs
                .Where(wl => wl.MemberID == 83
                    && DbFunctions.TruncateTime(wl.WorkoutTime) == d).OrderBy(wl => wl.WorkoutTime)
                .AsEnumerable()
                .Select(wl => new
                {
                    wl.ID,
                    wl.WorkoutTime,
                    wl.Workout.Name,
                    wl.WorkoutHours,
                    wl.StatusID,
                    Consume = EstimateCal(wl.WorkoutID, wl.WorkoutHours)
                });

            return Json(q.ToList());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult GetTodayConsume()
        {
            decimal q = TodayConsume();

            return Json(new { TodayConsume = q.ToString("0.00") });
        }


        //todo
        [NonAction]
        private decimal TodayConsume()
        {
            DateTime today = DateTime.Now.Date;
            DateTime tomorrow = today.AddDays(1);

            decimal weight = GetCurrentWeight(tomorrow);

            var q1 = dbContext.WorkoutLogs.Where(wl => wl.MemberID == 83
                    && DbFunctions.TruncateTime(wl.WorkoutTime) == today
                    && wl.StatusID == 5);

            if (q1.ToList().Count == 0)
            {
                return 0m;
            }

            double consume = q1.Sum(wl => wl.Workout.Calories * wl.WorkoutHours * (double)weight);

            return (decimal)consume;
        }

        //todo
        [NonAction]
        private decimal GetCurrentWeight(DateTime tomorrow)
        {
            var q1 = dbContext.WeightLogs
                .Where(wgt => wgt.MemberID == 83 && wgt.UpdatedDate < tomorrow)
                .OrderByDescending(wgt => wgt.UpdatedDate)
                .FirstOrDefault();

            decimal weight = (decimal)q1.Weight;
            return weight;
        }

        //todo
        [NonAction]
        private decimal TodayIngest()
        {
            DateTime today = DateTime.Now.Date;

            string strToday = today.ToString("MMddyyyy");

            var q1 = dbContext.DietLogs.Where(dt => dt.MemberID == 83
                && dt.Date == strToday);

            if (q1.ToList().Count == 0)
            {
                return 0m;
            }

            double ingest = q1.Sum(dt => dt.Portion * dt.MealOption.Calories);

            return (decimal)ingest;
        }

        //====================================================================
        //WorkoutPreferences Page

        public ActionResult WorkoutPreferences()
        {

            string JsonWc = JsonConvert.SerializeObject(dbContext.WorkoutCategories.Select(wc => new
            {
                wc.ID,
                wc.Name
            }).ToList());

            ViewData["JsonWc"] = JsonWc;

            string JsonWp = JsonConvert.SerializeObject(dbContext.WorkoutPreferences
                .Where(wp => wp.MemberID == 83).Select(wp => wp.WorkoutCategoryID).ToList());

            ViewData["JsonWp"] = JsonWp;

            string JsonW = JsonConvert.SerializeObject(dbContext.Workouts.Select(w => new
                {
                    wcid = w.WorkoutCategoryID,
                    Name = w.Name
                }).ToList());

            ViewData["JsonW"] = JsonW;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public string EditWp(int[] wps)
        {
            List<WorkoutPreference> wpList = dbContext.WorkoutPreferences.Where(wp => wp.MemberID == 83).ToList();

            for (int i = 0; i < wps.Length; i++)
            {
                if (wpList.SingleOrDefault(wp => wp.WorkoutCategoryID == wps[i]) == null)
                {
                    dbContext.WorkoutPreferences.Add(new WorkoutPreference
                    {
                        MemberID = 83,
                        WorkoutCategoryID = wps[i],
                    });
                }
            }

            foreach (var wp in wpList)
            {
                if (!wps.Contains(wp.WorkoutCategoryID))
                {
                    dbContext.WorkoutPreferences.Remove(wp);
                }
            }

            try
            {
                dbContext.SaveChanges();
            }
            catch
            {
                return "failed";
            }

            return "success";
        }
    }
}