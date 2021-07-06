using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
     public class DietLogEditDTO
    {
        public int DietLogID { get; set; }
        public int TimeOfDayID { get; set; }
        public double Portion { get; set; }

        public bool IsCustomUploaded { get; set; }

    }
}
