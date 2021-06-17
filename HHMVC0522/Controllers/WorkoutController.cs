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

namespace UI.Controllers
{
    [Authorize]
    public class WorkoutController : Controller
    {
        HealthHelperEntities dbContext = new HealthHelperEntities();

        public decimal TDEE
        {
            get
            {
                DateTime zeroTime = new DateTime(1, 1, 1);
                DateTime today = DateTime.Now.Date;
                DateTime tomorrow = today.AddDays(1);

                int MemberID = (int)Session["ID"];

                Member member = dbContext.Members.SingleOrDefault(m => m.ID == MemberID);
                decimal weight = GetCurrentWeight(tomorrow);
                int age = (zeroTime + (today - member.Birthdate)).Year - 1;
                decimal height = (decimal)member.Height;

                decimal TDEE;

                //todo when weight == 0, this TDEE is not right
                if (member.Gender)
                {
                    TDEE = 10 * weight + 6.25m * height + 5 * age - 5;
                }
                else
                {
                    TDEE = 10 * weight + 6.25m * height + 5 * age - 161;
                }

                return TDEE;
            }
        }

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

            int MemberID = (int)Session["ID"];

            var q1 = dbContext.WeightLogs
                .Where(wgt => wgt.MemberID == MemberID && wgt.UpdatedDate < tomorrow)
                .OrderByDescending(wgt => wgt.UpdatedDate)
                .FirstOrDefault();


            //decimal weight = (decimal)q1.Weight;

            var q = dbContext.WorkoutLogs.Where(wl => wl.MemberID == MemberID).ToList()
                .Select(wl => new
                {
                    wl.ID,
                    wl.WorkoutID,
                    wl.EditTime,
                    wl.Workout.Name,
                    wl.WorkoutTime,
                    wl.WorkoutHours,
                    Consume = ((decimal)(wl.Workout.Calories * wl.WorkoutHours) * GetCurrentWeight(wl.WorkoutTime)).ToString("0.00"),
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

            wl.MemberID = (int)Session["ID"];
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
            int MemberID = (int)Session["ID"];

            WorkoutLog workoutlog = dbContext.WorkoutLogs
                .SingleOrDefault(wl => wl.ID == ID && wl.MemberID == MemberID);

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
            workoutlog.MemberID = (int)Session["ID"];
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

        //todo QnA Maker是否可運用
        public ActionResult WorkoutSchedule()
        {
            //==================================================================
            //For chart
            decimal todayIngest = TodayIngest();

            TodayCaloriesToPercent(this.TDEE, todayIngest, TodayConsume(),
                out decimal TDEEPercent, out decimal IngestPercent, out decimal ConsumePercent);

            ViewBag.TDEE = TDEEPercent;
            ViewBag.Ingest = IngestPercent;
            ViewBag.Consume = ConsumePercent;

            //===================================================================
            //For suggest
            WorkoutSuggestViewModel vm = new WorkoutSuggestViewModel();

            DateTime today = DateTime.Now;

            string todayString = today.ToString("MMddyyyy");
            string yesterdayString = today.AddDays(-1).ToString("MMddyyyy");

            int[] timeArray = { 6, 11, 14, 17, 21 };

            for (int i = 0; i < 5; i++)
            {
                if (today.Hour >= timeArray[i])
                {
                    vm.IngestReport[i] = ingestPerTime(todayString, i + 1);
                }
            }

            if (today.Hour >= 6 && today.Hour < 11)
            {
                if (ingestPerTime(yesterdayString, 5) == 0)
                {
                    vm.Suggestion.Add("昨天沒吃宵夜，太讚了");
                }

                //vm.IngestReport[0] = ingestPerTime(todayString, 1);

                if (vm.IngestReport[0] == 0)
                {
                    vm.Suggestion.Add("還沒吃早餐喔，吃點東西在運動吧");
                }
                else if (vm.IngestReport[0] <= this.TDEE / 3)
                {
                    vm.Suggestion.Add("早餐吃得剛好");
                }
                else if (vm.IngestReport[0] > this.TDEE / 3)
                {
                    vm.Suggestion.Add("早餐吃的偏多，中午休息時去做個運動吧");
                }

            }
            
            if (today.Hour >= 11 && today.Hour < 14)
            {
                //vm.Suggestion.Clear();
                
                if (vm.IngestReport[0] == 0)
                {
                    vm.Suggestion.Add("要養成吃早餐的習慣喔");
                }

                //vm.IngestReport[1] = ingestPerTime(todayString, 2);

                if (vm.IngestReport[1] == 0)
                {
                    vm.Suggestion.Add("記得吃午餐喔");
                }
                else if (vm.IngestReport[1] <= this.TDEE / 3)
                {
                    vm.Suggestion.Add("午餐吃得剛好");
                }
                else if (vm.IngestReport[1] > this.TDEE / 3)
                {
                    vm.Suggestion.Add("午餐吃的偏多，下午去做個運動吧");
                }

            }
            
            if (today.Hour >= 14 && today.Hour < 17)
            {
                //vm.Suggestion.Clear();

                if (vm.IngestReport[1] == 0)
                {
                    vm.Suggestion.Add("哇，過了中午都還沒吃啊");
                }

                //vm.IngestReport[2] = ingestPerTime(todayString, 3);

                if (vm.IngestReport[2] == 0)
                {
                    vm.Suggestion.Add("不吃點心，也很OK");
                }
                else if (vm.IngestReport[2] <= this.TDEE / 5)
                {
                    vm.Suggestion.Add("偷吃一點點心，應該還好");
                }
                else if (vm.IngestReport[2] > this.TDEE / 5)
                {
                    vm.Suggestion.Add("點心吃太多了，一定要找時間運動啊");
                }

            }
            
            if (today.Hour >= 17 && today.Hour < 21)
            {
                //vm.Suggestion.Clear();

                if (vm.IngestReport[2] == 0)
                {
                    vm.Suggestion.Add("沒有偷吃點心，good");
                }

                //vm.IngestReport[3] = ingestPerTime(todayString, 4);

                if (vm.IngestReport[3] == 0)
                {
                    vm.Suggestion.Add("記得吃晚餐喔");
                }
                else if (vm.IngestReport[3] <= this.TDEE / 3)
                {
                    vm.Suggestion.Add("晚餐吃得剛好，讚");
                }
                else if (vm.IngestReport[3] > this.TDEE / 3)
                {
                    vm.Suggestion.Add("晚上暴吃一頓，明天一定要運動");
                }

            }
            
            if (today.Hour >= 21)
            {
                //vm.Suggestion.Clear();

                if (vm.IngestReport[3] == 0)
                {
                    vm.Suggestion.Add("沒吃晚餐，會不會睡不著啊");
                }

                //vm.IngestReport[4] = ingestPerTime(todayString, 5);

                if (vm.IngestReport[4] == 0)
                {
                    vm.Suggestion.Add("還沒吃宵夜，要忍住喔");
                }
                else if (vm.IngestReport[4] <= this.TDEE / 6)
                {
                    vm.Suggestion.Add("宵夜吃的也不算太多啦");
                }
                else if(vm.IngestReport[4] > this.TDEE / 6)
                {
                    vm.Suggestion.Add("宵夜吃那麼多，鐵定胖");
                }

            }

            int MemberID = (int)Session["ID"];

            vm.TimeOfDay = dbContext.TimesOfDays
                .OrderBy(tod => tod.ID).Select(tod => tod.Name).ToList();

            var q1 = dbContext.WorkoutPreferences
                .Where(wp => wp.MemberID == MemberID)
                .SelectMany(wp => wp.WorkoutCategory.Workouts);

            decimal warning = todayIngest / TDEE * 100;

            vm.Warning = $"為TDEE的 {warning:0.00}%";

            if (warning > 100)
            {
                vm.WorkoutSuggestion = q1.Where(w => w.ActivityLevelID == 3).ToList();
                vm.ActivityLevel = "【高強度】運動";
            }
            else if (warning > 90)
            {
                vm.WorkoutSuggestion = q1.Where(w => w.ActivityLevelID == 2).ToList();
                vm.ActivityLevel = "【中強度】運動";
            }
            else if (warning > 85)
            {
                vm.WorkoutSuggestion = q1.Where(w => w.ActivityLevelID == 1).ToList();
                vm.ActivityLevel = "【低強度】運動";
            }
            else
            {
                vm.Warning = "正常";
                vm.WorkoutSuggestion = dbContext.Workouts
                    .Where(w => w.ActivityLevelID == 6).ToList();
                vm.ActivityLevel = "【緩和】運動";
            }

            return View(vm);
        }

        private decimal ingestPerTime(string dayString, int timeOfDay)
        {
            int MemberID = (int)Session["ID"];

            var q1 = dbContext.DietLogs.Where(dl => dl.MemberID == MemberID
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
        private void TodayCaloriesToPercent(decimal TDEE, decimal todayIngest, decimal todayConsume,
            out decimal TDEEPercent, out decimal IngestPercent, out decimal ConsumePercent)
        {
            
            if (TDEE >= todayIngest && TDEE >= todayConsume)
            {
                TDEEPercent = 100;
                IngestPercent = Math.Round(todayIngest / TDEE * 100, 1);
                ConsumePercent = Math.Round(todayConsume / TDEE * 100, 1);
            }
            else if (todayIngest >= TDEE && todayIngest >= todayConsume)
            {
                IngestPercent = 100;
                ConsumePercent = Math.Round(todayConsume / todayIngest * 100, 1);
                TDEEPercent = Math.Round(TDEE / todayIngest * 100, 1);
            }
            else
            {
                ConsumePercent = 100;
                TDEEPercent = Math.Round(TDEE / todayConsume * 100, 1);
                IngestPercent = Math.Round(todayIngest / todayConsume * 100, 1);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult GetSchedule()
        {
            DateTime tomorrow = DateTime.Now.AddDays(1).Date;
            DateTime d8a = DateTime.Now.AddDays(8).Date;

            int MemberID = (int)Session["ID"];

            var q = dbContext.WorkoutLogs.Where(wl => wl.MemberID == MemberID
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

            decimal todayConsume = TodayConsume();

            TodayCaloriesToPercent(this.TDEE, TodayIngest(), todayConsume,
                out decimal TDEEPercent, out decimal IngestPercent, out decimal ConsumePercent);

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

            int MemberID = (int)Session["ID"];

            var q = dbContext.WorkoutLogs
                .Where(wl => wl.MemberID == MemberID
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


        [NonAction]
        private decimal TodayConsume()
        {
            DateTime today = DateTime.Now.Date;
            DateTime tomorrow = today.AddDays(1);

            decimal weight = GetCurrentWeight(tomorrow);

            int MemberID = (int)Session["ID"];

            var q1 = dbContext.WorkoutLogs.Where(wl => wl.MemberID == MemberID
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
        private decimal GetCurrentWeight(DateTime time)
        {
            //if there is no weightlog, set weight to 0 kg
            decimal weight = 0m;

            int MemberID = (int)Session["ID"];

            var q1 = dbContext.WeightLogs
                .Where(wgt => wgt.MemberID == MemberID && wgt.UpdatedDate < time)
                .OrderByDescending(wgt => wgt.UpdatedDate)
                .FirstOrDefault();

            if (q1 != null)
            {
                weight = (decimal)q1.Weight;
            }
            
            return weight;
        }

        //todo 采馨會改日期格式
        [NonAction]
        private decimal TodayIngest()
        {
            DateTime today = DateTime.Now.Date;

            string strToday = today.ToString("MMddyyyy");

            int MemberID = (int)Session["ID"];

            var q1 = dbContext.DietLogs.Where(dt => dt.MemberID == MemberID
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

            int MemberID = (int)Session["ID"];

            string JsonWp = JsonConvert.SerializeObject(dbContext.WorkoutPreferences
                .Where(wp => wp.MemberID == MemberID).Select(wp => wp.WorkoutCategoryID).ToList());

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
            if (wps == null)
            {
                wps = new int[] { };
            }
                
            int MemberID = (int)Session["ID"];

            List<WorkoutPreference> wpList = dbContext.WorkoutPreferences
                .Where(wp => wp.MemberID == MemberID).ToList();

            for (int i = 0; i < wps.Length; i++)
            {
                if (wpList.SingleOrDefault(wp => wp.WorkoutCategoryID == wps[i]) == null)
                {
                    dbContext.WorkoutPreferences.Add(new WorkoutPreference
                    {
                        MemberID = (int)Session["ID"],
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