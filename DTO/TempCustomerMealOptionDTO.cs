using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class TempCustomerMealOptionDTO
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public decimal Calories { get; set; }
        public string ImagePath { get; set; }
        public int StatusID { get; set; }
        public int MealOptionID { get; set; }
        public int MemberID { get; set; }
        public string EditTime { get; set; }
        public string Date { get; set; }
        public bool IsValid { get; set; }
        public int TimeOfDayID { get; set; }
        public float Portion { get; set; }
        public string UnitName { get; set; }
        public List<StatusDetailDTO> statusList { get; set; }
        public string stausName { get; set; }
        
    }
}
