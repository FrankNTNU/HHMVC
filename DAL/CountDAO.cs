using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class CountDAO : HHContext
    {
        public int GetUserCount()
        {
            int count = db.Members.Count();
            // 從資料庫抓到底有幾位會員，把結果丟給count變數
            return count;
        }
    }
}
