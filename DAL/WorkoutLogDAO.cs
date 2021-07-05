using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using DTO;
using System.Data.Entity;

namespace DAL
{
    public class WorkoutLogDAO : HHContext
    {
        public void DeleteByMemberID(int ID)
        {
            List<WorkoutLog> workoutLogs = db.WorkoutLogs.Where(x => x.MemberID == ID).ToList();
            db.WorkoutLogs.RemoveRange(workoutLogs);
            db.SaveChanges();
        }






        public double[] GetMonthlyBurnedCals(int memberId, DateTime date)
        {
            int howManyDays = DateTime.DaysInMonth(date.Year, date.Month);
            double[] datas = new double[howManyDays];
            var logsOfTheMonth = db.WorkoutLogs
                .Where(wl => wl.MemberID == memberId && wl.WorkoutTime.Month == date.Month && wl.WorkoutTime.Year == date.Year)
                .OrderBy(wl => wl.WorkoutTime).Select(wl =>
                 new {
                     day =wl.WorkoutTime.Day,
                     calPerKg =wl.Workout.Calories,
                     weight= wl.Member.WeightLogs.Where(wt=>wt.UpdatedDate<= date).OrderByDescending(wt => wt.UpdatedDate).FirstOrDefault(),
                     duration = wl.WorkoutHours,

                 }).Select(lg => new {
                     day = lg.day,
                     burnedCals = lg.calPerKg * lg.weight.Weight * lg.duration

                 });


            foreach (var log in logsOfTheMonth)
            {
                datas[log.day - 1] += log.burnedCals;
            }
      
            return datas;
        }

        public double Past7DaysWorkoutBurnedCalsFromDate(int memberId, DateTime date)
        {
            DateTime startDay = date.AddDays(-6);
            double burnCalsTotal =db.WorkoutLogs.Where(wo => wo.MemberID == memberId && DbFunctions.TruncateTime(wo.WorkoutTime) >= startDay && DbFunctions.TruncateTime(wo.WorkoutTime) <= date)
                 .Sum(wo => wo.Workout.Calories * wo.WorkoutHours * wo.Member.WeightLogs.Where(w => DbFunctions.TruncateTime(w.UpdatedDate) <= date).OrderByDescending(w => DbFunctions.TruncateTime(w.UpdatedDate)).Select(w => w.Weight).FirstOrDefault());
            return burnCalsTotal;
        }
    }
}
