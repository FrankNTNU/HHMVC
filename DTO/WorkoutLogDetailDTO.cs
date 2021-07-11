using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class WorkoutLogDetailDTO
    {
        public int ID { get; set; }
        public int MemberID { get; set; }
        public string MemberName { get; set; }
        public int WorkoutID { get; set; }
        public string WorkoutName { get; set; }
        public DateTime EditTime { get; set; }
        public double WorkoutHours { get; set; }
        public double Calories { get; set; }
    }
}
