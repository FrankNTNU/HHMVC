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

            return Json(new { Al = workout.ActivityLevel.Description, Wc = workout.WorkoutCategory.Name});
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult GetTodayWorkout()
        {

            DateTime d = DateTime.Now.Date;

            var q = dbContext.WorkoutLogs
                .Where(wl => wl.MemberID == 83 && wl.StatusID == 4
                    && DbFunctions.TruncateTime(wl.WorkoutTime) == d)
                .Select(wl => new
                {
                    wl.ID,
                    wl.WorkoutTime,
                    wl.Workout.Name,
                    wl.WorkoutHours
                });

            return Json(q.ToList());
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

            return Json(new { Result = "success", Error = "none" });

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


        //todo
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

        public JsonResult GetSchedule()
        {
            DateTime tomorrow = DateTime.Now.AddDays(1).Date;
            DateTime d8a = DateTime.Now.AddDays(8).Date;

            var q = dbContext.WorkoutLogs.Where(wl => wl.MemberID == 83
                && DbFunctions.TruncateTime(wl.WorkoutTime) >= tomorrow
                && DbFunctions.TruncateTime(wl.WorkoutTime) < d8a && wl.StatusID == 4)
                .Select(wl => new 
                { 
                    wl.WorkoutTime,
                    wl.Workout.Name,
                    wl.WorkoutHours
                }).OrderBy(wl => wl.WorkoutTime);

            return Json(q.ToList());
        }
    }
}