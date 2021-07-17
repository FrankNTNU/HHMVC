using DAL;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using UI.ViewModels;
using DTO;
using System.Globalization;

namespace UI.Controllers
{
    [Authorize]
    public class WorkoutController : Controller
    {
        HealthHelperEntities dbContext = new HealthHelperEntities();

        int[] timeArray = { 6, 11, 14, 17, 21 };

        //If user has a program, TDEE is calculated by initialWeight
        public static decimal TDEE(HealthHelperEntities dbContext, string UserID)
        {

            //================================================================
            //TDEE calculated by pal
            DateTime taipeiToday = DateTime.Now;

            var wgtList = dbContext.WeightLogs.OrderByDescending(wgtl => wgtl.UpdatedDate).ToList();
            var prgList = dbContext.Programs.Where(prg => DbFunctions.TruncateTime(prg.StartDate) <= taipeiToday.Date
                && DbFunctions.TruncateTime(prg.EndDate) >= taipeiToday.Date && prg.StatusID == 1)
                .OrderByDescending(prg => prg.StartDate);

            Member member = dbContext.Members.SingleOrDefault(m => m.ID.ToString() == UserID);

            decimal weight = 0;

            var program = prgList.SingleOrDefault(prg => prg.MemberID.ToString() == UserID);

            if (program != null)
            {
                weight = program.InitialWeight;
            }
            else
            {
                WeightLog wgtLog = wgtList.FirstOrDefault(wgt => wgt.MemberID.ToString() == UserID);
                if (wgtLog != null)
                {
                    weight = (decimal)wgtLog.Weight;
                }
            }

            //age
            DateTime zeroTime = new DateTime(1, 1, 1);
            int age = (zeroTime + (taipeiToday - member.Birthdate)).Year - 1;

            //pal
            decimal pal = 1.2m;

            int al = 0;

            if (program != null)
            {
                al = program.ActivityLevelID;
            }
            else
            {
                al = member.ActivityLevelID;
            }

            switch (al)
            {
                case 6:
                    pal = 1.2m;
                    break;
                case 1:
                    pal = 1.4m;
                    break;
                case 2:
                    pal = 1.6m;
                    break;
                case 3:
                    pal = 1.8m;
                    break;
            }

            //TDEE
            decimal TDEE = 0;
            if (member.Gender)
            {
                TDEE = (10 * weight + 6.25m * (decimal)member.Height - 5 * (decimal)age + 5) * pal;
            }
            else
            {
                TDEE = (10 * weight + 6.25m * (decimal)member.Height - 5 * (decimal)age - 161) * pal;
            }

            return TDEE;

        }

        //==========================================================
        //WorkoutLog Page

        public ActionResult WorkoutLog()
        {
            ViewBag.SuccessRate = calcSuccessRate();

            return View(dbContext.Workouts.ToList());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetWorkoutLog()
        {
            
            var q = dbContext.WorkoutLogs
                .Where(wl => wl.MemberID.ToString() == User.Identity.Name).ToList()
                .Select(wl =>
                {
                    bool isPreference = dbContext.WorkoutPreferences.Where(wp => wp.MemberID.ToString() == User.Identity.Name)
                        .Any(wp => wp.WorkoutCategoryID == wl.Workout.WorkoutCategoryID);

                    return new
                    {
                        wl.ID,
                        wl.WorkoutID,
                        wl.EditTime,
                        wl.Workout.Name,
                        wl.WorkoutTime,
                        wl.WorkoutHours,
                        Consume = ((decimal)(wl.Workout.Calories * wl.WorkoutHours) * GetCurrentWeight(wl.WorkoutTime)).ToString("0.00"),
                        Status = wl.Status.Name,
                        IsPreference = isPreference
                    };
                });

            return Json(q.ToList());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult GetWorkoutAlWc(int wid)
        {
            string UserID = User.Identity.Name;

            var workout = dbContext.Workouts.SingleOrDefault(w => w.ID == wid);

            bool isPreference = dbContext.WorkoutPreferences.Where(wp => wp.MemberID.ToString() == UserID)
                .Any(wp => wp.WorkoutCategoryID == workout.WorkoutCategoryID); ;

            string activityLevel = "";

            if (workout.ActivityLevelID == 1)
            {
                activityLevel = "【低強度】運動";
            }
            else if (workout.ActivityLevelID == 2)
            {
                activityLevel = "【中強度】運動";
            }
            else if (workout.ActivityLevelID == 3)
            {
                activityLevel = "【高強度】運動";
            }
            else if (workout.ActivityLevelID == 6)
            {
                activityLevel = "【緩和】運動";
            }

            return Json(new 
            { 
                Al = activityLevel, 
                Wc = workout.WorkoutCategory.Name,
                IsPreference = isPreference
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddWorkoutLog(WorkoutLog wl)
        {
            //======================================================================
            //check the time interval if overlapped
            string UserID = User.Identity.Name;
            int MemberID = int.Parse(UserID);

            DateTime today = DateTime.Now.Date;
            DateTime d8 = DateTime.Now.Date.AddDays(8);

            var wlList = dbContext.WorkoutLogs.Where(wl1 => wl1.MemberID.ToString() == UserID
                && DbFunctions.TruncateTime(wl1.WorkoutTime) >= today
                && DbFunctions.TruncateTime(wl1.WorkoutTime) < d8).ToList();

            DateTime workoutStart = wl.WorkoutTime;
            DateTime workoutEnd = workoutStart.AddHours(wl.WorkoutHours);

            foreach (var wl1 in wlList)
            {
                DateTime wStart = wl1.WorkoutTime;
                DateTime wEnd = wStart.AddHours(wl1.WorkoutHours);

                if (!((workoutStart < wStart && workoutEnd <= wStart) 
                    || (workoutStart >= wEnd && workoutEnd > wEnd)))
                {
                    return Json(new 
                    { 
                        Result = $"此運動時間區間已有其他紀錄\n(運動時間：{wl1.WorkoutTime}，運動項目：{wl1.Workout.Name})"
                    });
                }
            }

            //=====================================================================

            wl.MemberID = MemberID;
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

            return Json(new { Result = "success", Error = "none", ID = wl.ID, SuccessRate = calcSuccessRate() });

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult DeleteWorkoutLog(int ID)
        {
            string UserID = User.Identity.Name;

            WorkoutLog workoutlog = dbContext.WorkoutLogs
                .SingleOrDefault(wl => wl.ID == ID && wl.MemberID.ToString() == UserID);

            dbContext.WorkoutLogs.Remove(workoutlog);

            try
            {
                dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                return Json(new { Result = "failed", Error = ex.Message });
            }

            return Json(new { Result = "success", Error = "none", SuccessRate = calcSuccessRate() });

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult EditWorkoutLog(WorkoutLog wlToEdit)
        {
            //======================================================================
            //check the time interval if overlapped
            string UserID = User.Identity.Name;
            int MemberID = int.Parse(UserID);

            DateTime workoutStart = wlToEdit.WorkoutTime;
            DateTime workoutEnd = workoutStart.AddHours(wlToEdit.WorkoutHours);

            List<WorkoutLog> wlList = null;
            DateTime tomorrow = DateTime.Now.AddDays(1).Date;

            if (wlToEdit.StatusID == 4)
            {
                
                DateTime d8 = DateTime.Now.Date.AddDays(8);
                wlList = dbContext.WorkoutLogs.Where(wl1 => wl1.MemberID.ToString() == UserID
                    && DbFunctions.TruncateTime(wl1.WorkoutTime) >= tomorrow
                    && DbFunctions.TruncateTime(wl1.WorkoutTime) < d8).ToList();
            }
            else
            {
                DateTime d7b = DateTime.Now.Date.AddDays(-7);
                wlList = dbContext.WorkoutLogs.Where(wl1 => wl1.MemberID.ToString() == UserID
                    && DbFunctions.TruncateTime(wl1.WorkoutTime) < tomorrow
                    && DbFunctions.TruncateTime(wl1.WorkoutTime) >= d7b).ToList();
            }

            foreach (var wl1 in wlList)
            {
                if (wl1.ID == wlToEdit.ID)
                {
                    continue;
                }
                DateTime wStart = wl1.WorkoutTime;
                DateTime wEnd = wStart.AddHours(wl1.WorkoutHours);

                if (!((workoutStart < wStart && workoutEnd <= wStart)
                    || (workoutStart >= wEnd && workoutEnd > wEnd)))
                {
                    return Json(new
                    {
                        Result = $"此運動時間區間已有其他紀錄\n(運動時間：{wl1.WorkoutTime}，運動項目：{wl1.Workout.Name})"
                    });
                }
            }

            //======================================================================


            WorkoutLog workoutlog = dbContext.WorkoutLogs.SingleOrDefault(wl => wl.ID == wlToEdit.ID);
            workoutlog.MemberID = MemberID;
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

            return Json(new { Result = "success", Error = "none", SuccessRate = calcSuccessRate() });

        }

        private string calcSuccessRate()
        {
            var wlList = dbContext.WorkoutLogs
                .Where(wl => wl.MemberID.ToString() == User.Identity.Name);

            decimal wlCount = wlList.Where(wl => wl.StatusID == 5 || wl.StatusID == 6).Count();

            decimal successCount = wlList.Where(wl => wl.StatusID == 5).Count();

            return ((successCount / wlCount) * 100).ToString("#0.0") + "%";
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult GetEstimateCal(int wid, double whours, DateTime wTime)
        {
            return Json(new { Consume = EstimateCal(wid, whours, wTime) });
        }

        [NonAction]
        public string EstimateCal(int wid, double whours, DateTime wTime)
        {
            
            var cal = dbContext.Workouts.SingleOrDefault(w => w.ID == wid).Calories;
            var weight = GetCurrentWeight(wTime);

            return (cal * whours * (double)weight).ToString("0.00");
        }

        //======================================================
        //WorkoutSchedule Page
        public ActionResult WorkoutSchedule()
        {
            WorkoutSuggestViewModel vm = new WorkoutSuggestViewModel();

            //================================================================
            //ProgramWinner
            var winnerPrograms = dbContext.Programs.Where(prg => prg.StatusID == 5).ToList();

            List<WorkoutLog> winnerWorkoutLogs = new List<WorkoutLog>();

            winnerPrograms.ForEach(wprg => 
            {
                winnerWorkoutLogs.AddRange(dbContext.WorkoutLogs.Where(wl => wl.MemberID == wprg.MemberID
                    && DbFunctions.TruncateTime(wl.WorkoutTime) >= DbFunctions.TruncateTime(wprg.StartDate)
                    && DbFunctions.TruncateTime(wl.WorkoutTime) <= DbFunctions.TruncateTime(wprg.EndDate)).ToList());
            });

            //winnerFavorite
            vm.winnerFavorite = winnerWorkoutLogs
                .GroupBy(wl => wl.Workout).Select(group => new
                {
                    group.Key,
                    Count = group.Count()
                })
                .OrderByDescending(group => group.Count).Select(group => group.Key.Name).Take(5).ToList();

            //winnerDaysOfWeek
            CultureInfo myCI = new CultureInfo("en-US");
            Calendar cal = myCI.Calendar;

            vm.winnerDaysOfWeek = winnerWorkoutLogs
                .GroupBy(wl => new
                {
                    wl.MemberID,
                    wl.WorkoutTime.Year,
                    weekOfYear = cal.GetWeekOfYear(wl.WorkoutTime, CalendarWeekRule.FirstDay, DayOfWeek.Monday)
                })
                .Select(group => new
                {
                    Count = group.Count()
                })
                .Average(c => c.Count).ToString("0.00");

            //winnerHoursOfDay
            vm.winnerHoursOfDay = winnerWorkoutLogs
                .GroupBy(wl => new
                {
                    wl.MemberID,
                    wl.WorkoutTime.Date
                })
                .Select(group => new
                {
                    Hours = group.Sum(wl => wl.WorkoutHours)
                })
                .Average(h => h.Hours).ToString("0.00");


            DateTime today = DateTime.Now;
            string todayString = today.ToString("MMddyyyy");

            //==================================================================
            //Compute Now Ingest

            for (int i = 0; i < 5; i++)
            {
                if (today.Hour >= timeArray[i])
                {
                    vm.IngestReport[i] = ingestPerTime(todayString, i + 1);
                }
            }

            decimal nowIngest = 0m;

            foreach (var item in vm.IngestReport)
            {
                if (item != -1)
                {
                    nowIngest += item;
                }
            }

            //Compute Now Consume
            decimal nowConsume = consumeForNow();

            //==================================================================
            //For chart

            string UserID = User.Identity.Name;
            int MemberID = int.Parse(UserID);

            TodayCaloriesToPercent(TDEE(dbContext, UserID), nowIngest, nowConsume,
                out decimal TDEEPercent, out decimal IngestPercent, out decimal ConsumePercent);

            ViewBag.TDEE = TDEEPercent;
            ViewBag.Ingest = IngestPercent;
            ViewBag.Consume = ConsumePercent;

            //===================================================================
            //For suggest
            
            vm.TimeOfDay = dbContext.TimesOfDays
                .OrderBy(tod => tod.ID).Select(tod => tod.Name).ToList();

            decimal warning = (nowIngest - nowConsume) / TDEE(dbContext, UserID) * 100;
            vm.Warning = $"為TDEE的 {warning:##0}%";

            //===========================================================
            //SuggestionByPreferences
            var wpwList = dbContext.WorkoutPreferences
                .Where(wp => wp.MemberID.ToString() == UserID)
                .SelectMany(wp => wp.WorkoutCategory.Workouts);

            //SuggestionByLog
            var wlwList = dbContext.WorkoutLogs
                .Where(wl => wl.MemberID.ToString() == UserID)
                .GroupBy(wl => wl.Workout).Select(group => new
                {
                    group.Key,
                    ActivityLevelID = group.Key.ActivityLevelID,
                    Count = group.Count()
                }).OrderByDescending(w => w.Count);

            //SuggestionByAge
            var member = dbContext.Members.SingleOrDefault(m => m.ID.ToString() == UserID);
            DateTime zeroTime = new DateTime(1, 1, 1);
            int age = (zeroTime + (DateTime.Today - member.Birthdate)).Year - 1;
            int ageOffset = age % 10;
            int startBirth = member.Birthdate.AddYears(-ageOffset).Year;
            int endBirth = member.Birthdate.AddYears(10-ageOffset).Year;

            var mList = dbContext.Members.Where(m => 
                m.Birthdate.Year >= startBirth && m.Birthdate.Year <= endBirth
                && m.ID.ToString() != UserID)
                .Select(m => m.ID).ToList();

            var agewlList = dbContext.WorkoutLogs.Where(wl => mList.Contains(wl.MemberID))
                .GroupBy(wl => wl.Workout).Select(group => new
                {
                    group.Key,
                    ActivityLevelID = group.Key.ActivityLevelID,
                    Count = group.Count()
                }).OrderByDescending(w => w.Count);

            //Compute Threshold
            int TimeOfDay = Array.IndexOf(vm.IngestReport.ToArray(), -1);
            decimal timeCoefficient = 0m;
            switch (TimeOfDay)
            {
                case 0:
                    timeCoefficient = 0m;
                    break;
                case 1:
                    timeCoefficient = 0.2m;
                    break;
                case 2:
                    timeCoefficient = 0.5m;
                    break;
                case 3:
                    timeCoefficient = 0.6m;
                    break;
                case 4:
                    timeCoefficient = 0.9m;
                    break;
                //TimeOfDay == 5
                case -1:
                    timeCoefficient = 1m;
                    break;
            }

            if (timeCoefficient > 0m && warning > HHDictionary.HighActivitySuggestThreshold * timeCoefficient * 100)
            {
                vm.SuggestionByPreferences = wpwList.Where(w => w.ActivityLevelID == 3)
                    .Select(w => w.Name).OrderBy(w => Guid.NewGuid()).Take(5).ToList();
                vm.SuggestionByLog = wlwList.Where(w => w.ActivityLevelID == 3)
                    .Select(w => w.Key.Name).Take(5).ToList();
                vm.SuggestionByAge = agewlList.Where(w => w.ActivityLevelID == 3)
                    .Select(w => w.Key.Name).Take(5).ToList();
                vm.ActivityLevel = "【高強度】運動";
            }
            else if (timeCoefficient > 0m && warning > HHDictionary.MediumActivitySuggestThreshold * timeCoefficient * 100)
            {
                vm.SuggestionByPreferences = wpwList.Where(w => w.ActivityLevelID == 2)
                    .Select(w => w.Name).OrderBy(w => Guid.NewGuid()).Take(5).ToList();
                vm.SuggestionByLog = wlwList.Where(w => w.ActivityLevelID == 2)
                    .Select(w => w.Key.Name).Take(5).ToList();
                vm.SuggestionByAge = agewlList.Where(w => w.ActivityLevelID == 2)
                    .Select(w => w.Key.Name).Take(5).ToList(); ;
                vm.ActivityLevel = "【中強度】運動";
            }
            else if (timeCoefficient > 0m && warning > HHDictionary.LowActivitySuggestThreshold * timeCoefficient * 100)
            {
                vm.SuggestionByPreferences = wpwList.Where(w => w.ActivityLevelID == 1)
                    .Select(w => w.Name).OrderBy(w => Guid.NewGuid()).Take(5).ToList();
                //vm.SuggestionByPreferences.Clear();
                vm.SuggestionByLog = wlwList.Where(w => w.ActivityLevelID == 1)
                    .Select(w => w.Key.Name).Take(5).ToList(); ;
                //vm.SuggestionByLog.Clear();
                vm.SuggestionByAge = agewlList.Where(w => w.ActivityLevelID == 1)
                    .Select(w => w.Key.Name).Take(5).ToList();
                vm.ActivityLevel = "【低強度】運動";
            }
            else
            {
                vm.SuggestionByPreferences = dbContext.Workouts
                    .Where(w => w.ActivityLevelID == 6).Select(w => w.Name).ToList();
                vm.SuggestionByLog = dbContext.Workouts
                    .Where(w => w.ActivityLevelID == 6).Select(w => w.Name).ToList();
                vm.SuggestionByAge = dbContext.Workouts
                    .Where(w => w.ActivityLevelID == 6).Select(w => w.Name).ToList();
                vm.ActivityLevel = "【緩和】運動";
            }

            return View(vm);
        }


        [NonAction]
        private decimal ingestPerTime(string dayString, int timeOfDay)
        {
            string UserID = User.Identity.Name;

            var q1 = dbContext.DietLogs.Where(dl => dl.MemberID.ToString() == UserID
                    && dl.Date == dayString
                    && dl.TimeOfDayID == timeOfDay);

            decimal ingest = 0;

            if (q1.Count() > 0)
            {
                ingest = (decimal)(q1.Sum(dl => dl.Portion * dl.MealOption.Calories));
            }

            return ingest;
        }

        [NonAction]
        private decimal ingestForNow()
        {
            decimal ingest = 0m;

            for (int i = 0; i < timeArray.Length; i++)
            {
                if (DateTime.Now.Hour >= timeArray[i])
                {
                    ingest += ingestPerTime(DateTime.Today.ToString("MMddyyyy"), i + 1);
                }
            }

            return ingest;
        }

        [NonAction]
        private decimal consumeForNow()
        {
            string UserID = User.Identity.Name;

            decimal weight = GetCurrentWeight(DateTime.Now);

            var q1 = dbContext.WorkoutLogs.Where(wl => wl.MemberID.ToString() == UserID
                    && DbFunctions.TruncateTime(wl.WorkoutTime) == DateTime.Today
                    && wl.StatusID == 5).ToList();

            var q2 = q1.Where(wl => wl.WorkoutTime <= DateTime.Now);

            decimal consume = 0;

            if (q2.Count() > 0)
            {
                consume = q2.Sum(wl =>
                   (decimal)(wl.WorkoutHours * wl.Workout.Calories) * weight);
            }

            return consume;
        }

        [NonAction]
        private void TodayCaloriesToPercent(decimal TDEE, decimal nowIngest, decimal todayConsume,
            out decimal TDEEPercent, out decimal IngestPercent, out decimal ConsumePercent)
        {
            
            if (TDEE >= nowIngest && TDEE >= todayConsume)
            {
                TDEEPercent = 100;
                IngestPercent = Math.Round(nowIngest / TDEE * 100, 1);
                ConsumePercent = Math.Round(todayConsume / TDEE * 100, 1);
            }
            else if (nowIngest >= TDEE && nowIngest >= todayConsume)
            {
                IngestPercent = 100;
                ConsumePercent = Math.Round(todayConsume / nowIngest * 100, 1);
                TDEEPercent = Math.Round(TDEE / nowIngest * 100, 1);
            }
            else
            {
                ConsumePercent = 100;
                TDEEPercent = Math.Round(TDEE / todayConsume * 100, 1);
                IngestPercent = Math.Round(nowIngest / todayConsume * 100, 1);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult GetSchedule()
        {
            DateTime tomorrow = DateTime.Now.AddDays(1).Date;
            DateTime d8a = DateTime.Now.AddDays(8).Date;

            string UserID = User.Identity.Name;

            var q = dbContext.WorkoutLogs.Where(wl => wl.MemberID.ToString() == UserID
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

            decimal nowConsume = consumeForNow();

            string UserID = User.Identity.Name;

            TodayCaloriesToPercent(TDEE(dbContext, UserID), ingestForNow(), nowConsume,
                out decimal TDEEPercent, out decimal IngestPercent, out decimal ConsumePercent);

            return Json(new 
            { 
                Result = "success", 
                Error = "none", 
                TDEEPercent = TDEEPercent,
                IngestPercent = IngestPercent,
                ConsumePercent = ConsumePercent,
                TodayConsume = nowConsume
            });

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult GetTodayWorkout()
        {

            DateTime d = DateTime.Now.Date;

            string UserID = User.Identity.Name;

            var q = dbContext.WorkoutLogs
                .Where(wl => wl.MemberID.ToString() == UserID
                    && DbFunctions.TruncateTime(wl.WorkoutTime) == d).OrderBy(wl => wl.WorkoutTime)
                .ToList()
                .Select(wl => new
                {
                    wl.ID,
                    wl.WorkoutTime,
                    wl.Workout.Name,
                    wl.WorkoutHours,
                    wl.StatusID,
                    Consume = EstimateCal(wl.WorkoutID, wl.WorkoutHours, wl.WorkoutTime)
                });

            return Json(q.ToList());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult GetTodayInfo()
        {
            decimal ingest = ingestForNow();
            decimal consume = consumeForNow();

            string UserID = User.Identity.Name;

            return Json(new 
            {
                TDEE = TDEE(dbContext, UserID).ToString("0.00"),
                TodayIngest = ingest.ToString("0.00"),
                TodayConsume = consume.ToString("0.00") 
            });
        }


        [NonAction]
        private decimal TodayConsume()
        {
            DateTime today = DateTime.Now.Date;
            DateTime tomorrow = today.AddDays(1);

            decimal weight = GetCurrentWeight(tomorrow);

            string UserID = User.Identity.Name;

            var q1 = dbContext.WorkoutLogs.Where(wl => wl.MemberID.ToString() == UserID
                    && DbFunctions.TruncateTime(wl.WorkoutTime) == today
                    && wl.StatusID == 5);

            if (q1.ToList().Count == 0)
            {
                return 0m;
            }

            double consume = q1.Sum(wl => wl.Workout.Calories * wl.WorkoutHours * (double)weight);

            return (decimal)consume;
        }

        
        [NonAction]
        private decimal GetCurrentWeight(DateTime moment)
        {
            //if there is no weightlog, set weight to 0 kg
            decimal weight = 0m;

            string UserID = User.Identity.Name;

            var prgList = dbContext.Programs.Where(prg => DbFunctions.TruncateTime(prg.StartDate) <= moment.Date
                && DbFunctions.TruncateTime(prg.EndDate) >= moment.Date && prg.StatusID == 1)
                .OrderByDescending(prg => prg.StartDate);

            var program = prgList.SingleOrDefault(prg => prg.MemberID.ToString() == UserID);

            if (program != null)
            {
                weight = program.InitialWeight;
            }
            else
            {
                var weightLog = dbContext.WeightLogs
                    .Where(wgtl => wgtl.UpdatedDate <= moment)
                    .OrderByDescending(wgtl => wgtl.UpdatedDate)
                    .FirstOrDefault(wgt => wgt.MemberID.ToString() == UserID);

                if (weightLog == null)
                {
                    return 0m;
                }
                else 
                {
                    weight = (decimal)weightLog.Weight;
                }
            }

            return weight;
        }

        //todo 采馨會改日期格式
        [NonAction]
        private decimal TodayIngest()
        {
            DateTime today = DateTime.Now.Date;

            string strToday = today.ToString("MMddyyyy");

            string UserID = User.Identity.Name;

            var q1 = dbContext.DietLogs.Where(dt => dt.MemberID.ToString() == UserID
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
            }).OrderBy(wc => wc.ID).ToList());

            ViewData["JsonWc"] = JsonWc;

            string UserID = User.Identity.Name;

            string JsonWp = JsonConvert.SerializeObject(dbContext.WorkoutPreferences
                .Where(wp => wp.MemberID.ToString() == UserID).Select(wp => wp.WorkoutCategoryID).ToList());

            ViewData["JsonWp"] = JsonWp;

            string JsonW = JsonConvert.SerializeObject(dbContext.Workouts.Select(w => new
                {
                    wcid = w.WorkoutCategoryID,
                    Name = w.Name
                }).OrderBy(w => w.wcid).ToList());

            ViewData["JsonW"] = JsonW;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public string EditWp(int[] wps)
        {
            if (wps == null)
            {
                wps = new int[] { };
            }

            string UserID = User.Identity.Name;
            int MemberID = int.Parse(UserID);

            List<WorkoutPreference> wpList = dbContext.WorkoutPreferences
                .Where(wp => wp.MemberID.ToString() == UserID).ToList();

            for (int i = 0; i < wps.Length; i++)
            {
                if (wpList.SingleOrDefault(wp => wp.WorkoutCategoryID == wps[i]) == null)
                {
                    dbContext.WorkoutPreferences.Add(new WorkoutPreference
                    {
                        MemberID = MemberID,
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
                if (wps.Length == 0)
                {
                    Session["NoPreferences"] = true;
                }
                else
                {
                    Session["NoPreferences"] = false;
                }
            }
            catch
            {
                return "failed";
            }

            return "success";
        }

        //=========================================================
        //Workout places
        public ActionResult WorkoutPlaces(string workout) 
        {
            string keyword = "健身房";

            if (!string.IsNullOrEmpty(workout))
            {
                keyword = ToPlacesKeyword(workout);
            }
            
            ViewBag.Keyword = keyword;
            ViewBag.Title = $"附近的{keyword}";

            return View();
        }

        [NonAction]
        public string ToPlacesKeyword(string workout)
        {
            if (workout.Contains("跑")
                || workout.Contains("步")
                || workout.Contains("走")
                || workout == "衝刺")
            {
                return "公園";
            }
            else if (workout.Contains("樓梯"))
            {
                return "登山步道";
            }
            else if (workout.Contains("騎"))
            {
                return "自行車道";
            }
            else if (workout.Contains("瑜珈"))
            {
                return workout;
            }
            else if (workout.Contains("舞蹈"))
            {
                return "舞蹈教室";
            }
            else if (workout.Contains("游"))
            {
                return "游泳池";
            }
            else if (workout.Contains("跳繩"))
            {
                return "公園";
            }
            else if (workout.Contains("球"))
            {
                return workout + "場";
            }
            else if (workout.Contains("有氧") || workout.Contains("飛輪"))
            {
                return "健身房";
            }
            else if (workout == "五行健康操")
            {
                return workout;
            }
            else
            {
                return null;
            }

        }
    }
}