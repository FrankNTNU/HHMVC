using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class CDictionary
    {
        public static readonly string KEY_DIETLOGCART_ITEMS = "KEY_DIETLOGCART_ITEMS";
        public static readonly string yyyyMMddHHmmss = "yyyyMMddHHmmss";
        public static readonly string MMddyyyy = "MMddyyyy";
        public static readonly string MMSlashddSlashyyyy = "MM/dd/yyyy";

        
        public static readonly int Breakfast =1 ;
        public static readonly int Lunch = 2;
        public static readonly int Snack = 3;
        public static readonly int Dinner = 4;
        public static readonly int BedtimeSnack = 5;
        public static readonly double LightMealCoefficient = 0.9; //todo
        public static readonly int StatusPendingReview = 2;
        public static readonly int StatusAprove = 13;
        public static readonly int BowlOfRiceCals = 280;
        public static readonly double DailyGainCalMinCoefficient = 0.8;






    }
}
