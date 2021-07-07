using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
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
        public List<WorkoutLogDTO> GetWorkoutLogs()
        {
            List<WorkoutLog> workoutLogs = db.WorkoutLogs.ToList();
            List<WorkoutLogDTO> logDTOlist = new List<WorkoutLogDTO>();
            foreach (var item in workoutLogs)
            {
                WorkoutLogDTO dto = new WorkoutLogDTO();
                dto.ID = item.ID;
                dto.MemberID = item.MemberID;
                dto.MemberName = item.Member.Name;
                dto.WorkoutID = item.WorkoutID;
                dto.WorkoutName = item.Workout.Name;
                dto.WorkoutHours = item.WorkoutHours;
                dto.Calories = Math.Round(item.Workout.Calories * item.WorkoutHours, 2);
                dto.EditTime = item.EditTime;
                logDTOlist.Add(dto);
            }
            return logDTOlist;
        }

        public List<WorkoutLogDTO> GetWorkoutLogs(string keyword)
        {
            List<WorkoutLog> workoutLogs = db.WorkoutLogs.Where(x => x.Workout.Name.Contains(keyword)
                || x.Member.Name.Contains(keyword)
                || x.ID.ToString().Contains(keyword)
                || x.MemberID.ToString().Contains(keyword)
                || x.WorkoutID.ToString().Contains(keyword)
                || x.EditTime.ToString().Contains(keyword)
                ).ToList();
            List<WorkoutLogDTO> logDTOlist = new List<WorkoutLogDTO>();
            foreach (var item in workoutLogs)
            {
                WorkoutLogDTO dto = new WorkoutLogDTO();
                dto.ID = item.ID;
                dto.MemberID = item.MemberID;
                dto.MemberName = item.Member.Name;
                dto.WorkoutName = item.Workout.Name;
                dto.WorkoutHours = item.WorkoutHours;
                dto.Calories = Math.Round(item.Workout.Calories * item.WorkoutHours, 2);
                logDTOlist.Add(dto);
            }
            return logDTOlist;
        }

        public WorkoutLogDTO GetWorkoutLogWithID(int ID)
        {
            WorkoutLogDTO dto = new WorkoutLogDTO();
            WorkoutLog log = db.WorkoutLogs.FirstOrDefault(x => x.ID == ID);
            if (log != null)
            {
                dto.ID = log.ID;
                dto.MemberName = log.Member.Name;
                dto.WorkoutID = log.WorkoutID;
                dto.WorkoutName = log.Workout.Name;
                dto.WorkoutHours = log.WorkoutHours;
                //dto.Calories = log.Workout.Calories;
            }
            return dto;
        }

        public List<WorkoutLogDTO> GetWorkoutLogs(DateTime starttime, DateTime end)
        {
            List<WorkoutLog> workoutLogs = db.WorkoutLogs.Where(x => x.EditTime >= starttime && x.EditTime <= end).ToList();

            List<WorkoutLogDTO> logDTOlist = new List<WorkoutLogDTO>();
            foreach (var item in workoutLogs)
            {
                WorkoutLogDTO dto = new WorkoutLogDTO();
                dto.MemberID = item.MemberID;
                dto.MemberName = item.Member.Name;
                dto.WorkoutName = item.Workout.Name;
                dto.WorkoutHours = item.WorkoutHours;
                dto.Calories = Math.Round(item.Workout.Calories * item.WorkoutHours, 2);
                logDTOlist.Add(dto);
            }
            return logDTOlist;
        }
        public void Update(WorkoutLog entity)
        {
            WorkoutLog workoutLog = db.WorkoutLogs.First(x => x.ID == entity.ID);
            workoutLog.WorkoutID = entity.WorkoutID;
            //workoutLog.Workout.Name = entity.Workout.Name;
            workoutLog.EditTime = entity.EditTime;
            workoutLog.WorkoutHours = entity.WorkoutHours;
            //workoutLog.Workout.Calories = entity.Workout.Calories;
            db.SaveChanges();
        }
        public bool Delete(int ID)
        {
            WorkoutLog workoutLog = db.WorkoutLogs.First(x => x.ID == ID);
            db.WorkoutLogs.Remove(workoutLog);
            db.SaveChanges();
            return true;
        }
        public bool Add(WorkoutLog workoutLog)
        {
            db.WorkoutLogs.Add(workoutLog);
            db.SaveChanges();
            return true;
        }

        public bool IsWorkoutLogExist(int ID)
        {
            try
            {
                WorkoutLog workoutLog = db.WorkoutLogs.FirstOrDefault(x => x.ID == ID);
                if (workoutLog != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public double[] GetMonthlyBurnedCals(int memberId, DateTime date)
        {
            int howManyDays = DateTime.DaysInMonth(date.Year, date.Month);
            double[] datas = new double[howManyDays];
            var logsOfTheMonth = db.WorkoutLogs
                .Where(wl => wl.MemberID == memberId && wl.WorkoutTime.Month == date.Month && wl.WorkoutTime.Year == date.Year)
                .OrderBy(wl => wl.WorkoutTime).Select(wl =>
                 new {
                     day = wl.WorkoutTime.Day,
                     calPerKg = wl.Workout.Calories,
                     weight = wl.Member.WeightLogs.Where(wt => wt.UpdatedDate <= date).OrderByDescending(wt => wt.UpdatedDate).FirstOrDefault(),
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
            double burnCalsTotal = db.WorkoutLogs.Where(wo => wo.MemberID == memberId && DbFunctions.TruncateTime(wo.WorkoutTime) >= startDay && DbFunctions.TruncateTime(wo.WorkoutTime) <= date)
                 .Sum(wo => wo.Workout.Calories * wo.WorkoutHours * wo.Member.WeightLogs.Where(w => DbFunctions.TruncateTime(w.UpdatedDate) <= date).OrderByDescending(w => DbFunctions.TruncateTime(w.UpdatedDate)).Select(w => w.Weight).FirstOrDefault());
            return burnCalsTotal;
        }
    }
}
