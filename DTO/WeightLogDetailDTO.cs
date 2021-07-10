using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class WeightLogDetailDTO
    {
        public int ID { get; set; }
        public int MemberID { get; set; }
        public string MemberName { get; set; }
        public double Weight { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}
