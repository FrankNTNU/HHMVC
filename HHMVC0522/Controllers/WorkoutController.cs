using DAL;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace UI.Controllers
{

    public class WorkoutController : Controller
    {
        HealthHelperEntities dbContext = new HealthHelperEntities();

        public ActionResult WorkoutLog()
        {
            return View(dbContext.Workouts.ToList());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetWorkoutLog()
        {
            var q = dbContext.WorkoutLogs.Where(wl => wl.MemberID == 83)
                .Select(wl => new
                {
                    wl.ID,
                    wl.WorkoutID,
                    wl.EditTime,
                    wl.Workout.Name,
                    wl.WorkoutTime,
                    wl.WorkoutHours,
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




        //======================================================
        //Today Workout Page

        //todo
        public ActionResult WorkoutSchedule()
        {
            decimal TDEE = 2000;
            decimal todayIngest = TodayIngest();
            decimal todayConsume = TodayConsume();

            if (TDEE >= todayIngest && TDEE >= TodayConsume())
            {
                ViewBag.TDEE = 100;
                ViewBag.Ingest = Math.Round(todayIngest / TDEE * 100, 1);
                ViewBag.Consume = Math.Round(todayConsume / TDEE * 100, 1);
            }
            else if (todayIngest >= TDEE && todayIngest >= todayConsume)
            {
                ViewBag.Ingest = 100;
                ViewBag.Consume = Math.Round(todayConsume / todayIngest * 100, 1);
                ViewBag.TDEE = Math.Round(TDEE / todayIngest * 100, 1);
            }
            else
            {
                ViewBag.Consume = 100;
                ViewBag.TDEE = Math.Round(TDEE / todayConsume * 100, 1);
                ViewBag.Ingest = Math.Round(todayIngest / todayConsume * 100, 1);
            }

            return View();
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

            int TDEE = 2000;

            decimal todayConsume = TodayConsume();

            decimal ConsumePercent = Math.Round(todayConsume / TDEE * 100, 1);

            return Json(new { Result = "success", Error = "none", ConsumePercent = ConsumePercent });

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult GetTodayWorkout()
        {

            DateTime d = DateTime.Now.Date;

            var q = dbContext.WorkoutLogs
                .Where(wl => wl.MemberID == 83
                    && DbFunctions.TruncateTime(wl.WorkoutTime) == d).OrderBy(wl => wl.WorkoutTime)
                .Select(wl => new
                {
                    wl.ID,
                    wl.WorkoutTime,
                    wl.Workout.Name,
                    wl.WorkoutHours,
                    wl.StatusID
                });

            return Json(q.ToList());
        }

        public JsonResult GetTodayConsume()
        {
            decimal q = TodayConsume();

            return Json(new { TodayConsume = q.ToString("0.00") });
        }


        //todo 消耗熱量需乘上會員體重
        [NonAction]
        private decimal TodayConsume()
        {
            DateTime today = DateTime.Now.Date;

            DateTime tomorrow = today.AddDays(1);

            var q1 = dbContext.WeightLogs
                .Where(wgt => wgt.MemberID == 83 && wgt.UpdatedDate < tomorrow)
                .OrderByDescending(wgt => wgt.UpdatedDate)
                .FirstOrDefault();

            decimal weight = (decimal)q1.Weight;

            var q2 = dbContext.WorkoutLogs.Where(wl => wl.MemberID == 83
                    && DbFunctions.TruncateTime(wl.WorkoutTime) == today
                    && wl.StatusID == 5)
                .Sum(wl => wl.Workout.Calories * wl.WorkoutHours * (double)weight);

            return (decimal)q2;
        }

        //todo
        [NonAction]
        private decimal TodayIngest()
        {
            DateTime today = DateTime.Now.Date;

            string strToday = today.ToString("MMddyyyy");

            var q1 = dbContext.DietLogs.Where(dt => dt.MemberID == 83
                && dt.Date == strToday)
                .Sum(dt => dt.Portion * dt.MealOption.Calories);

            return (decimal)q1;
        }
    }
}