using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class DietLogDTO
    {
        public int ID { get; set; }
        public int MemberID { get; set; }
        public int TimeOfDayID { get; set; }
        public string EditTime { get; set; }
        public float Portion { get; set; }
        public int MealOptionID { get; set; }
        public string Date { get; set; }
        public bool IsValid { get; set; }

    }
}
