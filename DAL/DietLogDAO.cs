using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class DietLogDAO : HHContext
    {
        public void DeleteByMemberID(int ID)
        {
            List<DietLog> dietLogs = db.DietLogs.Where(x => x.MemberID == ID).ToList();
            db.DietLogs.RemoveRange(dietLogs);
            db.SaveChanges();
        }
    }
}
