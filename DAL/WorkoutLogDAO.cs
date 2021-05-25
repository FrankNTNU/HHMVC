using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
