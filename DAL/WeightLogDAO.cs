using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class WeightLogDAO : HHContext
    {
        public void DeleteByMemberID(int ID)
        {
            List<WeightLog> weightLogs = db.WeightLogs.Where(x => x.MemberID == ID).ToList();
            db.WeightLogs.RemoveRange(weightLogs);
            db.SaveChanges();
        }
        public void AddWeightLog(WeightLog weightLog)
        {
            db.WeightLogs.Add(weightLog);
            db.SaveChanges();        
        }
    }
}
