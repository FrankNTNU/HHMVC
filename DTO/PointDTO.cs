using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class PointDTO
    {
        public int MemberID { get; set; }
        public string MemberName { get; set; }
        public int Points { get; set; }
        public DateTime AddTime { get; set; }
        public int StatusID { get; set; }
        public string StatusName { get; set; }
    }
}
