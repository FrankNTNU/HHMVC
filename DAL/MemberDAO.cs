using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class MemberDAO : HHContext
    {
        public Member GetMemberByMemberID(int memberID)
        {
            return db.Members.FirstOrDefault(m => m.ID == memberID);
        }

        public void UpdateActivityLevel(int memberID, int activityLevelId)
        {
            int x = db.Members.FirstOrDefault(m => m.ID == memberID).ActivityLevelID;
            db.Members.FirstOrDefault(m => m.ID == memberID).ActivityLevelID = activityLevelId;
            db.SaveChanges();
        }
    }
}
