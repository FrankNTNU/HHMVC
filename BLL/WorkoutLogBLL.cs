using DAL;
using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class WorkoutLogBLL
    {
        WorkoutLogDAO dao = new WorkoutLogDAO();
        
        public void DeleteByMemberID(int ID)
        {
            dao.DeleteByMemberID(ID);
        }

        public List<WorkoutLogDTO> GetWorkoutLogs()
        {
            List<WorkoutLogDTO> logList = new List<WorkoutLogDTO>();
            logList = dao.GetWorkoutLogs();
            return logList;
        }

        public List<WorkoutLogDTO> GetWorkoutLogs(string keyword)
        {
            List<WorkoutLogDTO> logList = new List<WorkoutLogDTO>();
            logList = dao.GetWorkoutLogs(keyword);
            return logList;
        }
        public void UpDate(WorkoutLogDTO entity)
        {
            WorkoutLog workoutLog = new WorkoutLog();
            workoutLog.ID = entity.ID;
            workoutLog.WorkoutID = (int)entity.WorkoutID;
            workoutLog.EditTime = DateTime.Now;
            //workoutLog.Workout.Name = entity.WorkoutName;
            workoutLog.WorkoutHours = entity.WorkoutHours;
            dao.Update(workoutLog);
        }

        public WorkoutLogDTO GetWorkoutLogWithID(int ID)
        {
            return dao.GetWorkoutLogWithID(ID);
        }

        public bool Delete(int ID)
        {
            return dao.Delete(ID);
        }

        public bool Add(WorkoutLogDTO entity)
        {
            WorkoutLog workoutLog = new WorkoutLog();
            workoutLog.ID = entity.ID;
            workoutLog.MemberID = (int)entity.MemberID;
            workoutLog.Member.Name = entity.MemberName;
            workoutLog.WorkoutID = (int)entity.WorkoutID;
            workoutLog.EditTime = entity.EditTime;
            workoutLog.Workout.Name = entity.WorkoutName;
            workoutLog.WorkoutHours = entity.WorkoutHours;
            return dao.Add(workoutLog);
        }

        public bool IsWorkoutLogExist(int ID)
        {
            return dao.IsWorkoutLogExist(ID);
        }


        public double Past7DaysWorkoutBurnedCalsFromDate(int memberId, DateTime date )
        {
            return dao.Past7DaysWorkoutBurnedCalsFromDate(memberId, date);
        }
        public double[] GetMonthlyBurnedCals(int memberId, DateTime date)
        {
            return dao.GetMonthlyBurnedCals(memberId, date);
        }



    }
}
