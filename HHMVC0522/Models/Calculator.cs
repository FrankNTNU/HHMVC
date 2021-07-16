using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UI.Models
{
    static public class Calculator
    {
        public static string[] GetNearbyWeekDays(DateTime date)
        {
            string[] result = new string[7];
            int n = (int)date.DayOfWeek;
            for (int i= 0; i < 7; i++)
            {
                result[i] = GetCharWeekDayInChinese((n + (4 + i)) % 7);
            }
            return result;
        }
        public static string[] GetPastWeekDaysFromTheDate(DateTime date)
        {
            string[] result = new string[7];
            int n = (int)date.DayOfWeek;
            for (int i = 0; i < 7; i++)
            {
                result[i] = GetCharWeekDayInChinese((n + (1 + i)) % 7);
            }
            return result;
        }
        private static string GetCharWeekDayInChinese(int weekOfDayNum)
        {
            switch (weekOfDayNum)
            {
                case 0: return "日";
                case 1: return "一";
                case 2: return "二";
                case 3: return "三";
                case 4: return "四";
                case 5: return "五";
                case 6: return "六";
                default: return "";
            }
        }

      
    }
}