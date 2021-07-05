using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class WorkoutLogBLL
    {
        WorkoutLogDAO workoutLogDAO = new WorkoutLogDAO();
        public void DeleteByMemberID(int ID)
        {
            workoutLogDAO.DeleteByMemberID(ID);
        }


        public double Past7DaysWorkoutBurnedCalsFromDate(int memberId, DateTime date )
        {
            return workoutLogDAO.Past7DaysWorkoutBurnedCalsFromDate(memberId, date);
        }
        public double[] GetMonthlyBurnedCals(int memberId, DateTime date)
        {
            return workoutLogDAO.GetMonthlyBurnedCals(memberId, date);
        }



    }
}
