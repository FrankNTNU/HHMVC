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
    }
}
