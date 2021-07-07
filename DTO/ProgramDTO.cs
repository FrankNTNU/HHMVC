using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class ProgramDTO
    {
        //public int ID { get; set; }
        //public int MemberID { get; set; }
        //public int StatusID { get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int TargetWeight { get; set; }
        //public int ActivityLevelID { get; set; }
       public int InitialWeight { get; set; }
        //public Nullable<int> ResultWeight { get; set; }

    }
}
