using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DTO
{
    public class MixedDietLogDTO
    {



        public int MemberID { get; set; }
        public int TimeOfDayID { get; set; }
        public double Portion { get; set; }
        public int MealOptionID { get; set; }
        public string Date { get; set; }

        //below for customer upload meal
        public string Name { get; set; }
        public double Calories { get; set; }

        public string UnitName { get; set; }

        public string ImagePath { get; set; }


       


    };


}

