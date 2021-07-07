using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace DTO
{
    public class WorkoutLogDTO
    {
        public int ID { get; set; }
        public int MemberID { get; set; }
        public string MemberName { get; set; }
        public int WorkoutID { get; set; }
        public string WorkoutName { get; set; }
        public DateTime EditTime { get; set; } 
        public double WorkoutHours { get; set; }
        public double Calories { get; set; }

        public IEnumerable<SelectListItem> WorkoutNames { get; set; }
    }
}
