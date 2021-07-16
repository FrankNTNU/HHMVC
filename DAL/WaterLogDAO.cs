using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.Data.Entity;

namespace DAL
{
    public class WaterLogDAO : HHContext
    {
        WeightLogDAO wDao = new WeightLogDAO();
        public bool HasWaterLogByDate(int memberId, string date)
        {
            return db.WaterLogs.Any(wl => wl.MemberID == memberId && wl.Date == date);
        }
        public void UpdateWaterLog(WaterLog waterLogAmountToAdded)
        {
            string today = DateTime.Now.ToString(CDictionary.MMddyyyy);
            WaterLog log = db.WaterLogs.FirstOrDefault(wl => wl.MemberID == waterLogAmountToAdded.MemberID
            && wl.Date == today);
            if (log != null)
            {
                int newAmount = log.WaterAmount + waterLogAmountToAdded.WaterAmount;
                if (newAmount <= 0) { db.WaterLogs.Remove(log); }
                else
                {
                    log.WaterAmount = newAmount;
                }
            }
            else
            {
                waterLogAmountToAdded.Date = today;
                db.WaterLogs.Add(waterLogAmountToAdded);
            }
            db.SaveChanges();

        }

        public int[] GetMonthlyGainedWater(int memberId, DateTime date)
        {
            int howManyDays = DateTime.DaysInMonth(date.Year, date.Month);
            DateTime theDate = new DateTime(date.Year, date.Month, 1);
            string[] dates = new string[howManyDays];
            for (int i = 0; i < howManyDays; i++)
            {
                dates[i] = theDate.AddDays(i).ToString(CDictionary.MMddyyyy);
            }
            int[] result = new int[howManyDays];
            var logsOfTheMonth = db.WaterLogs
                .Where(wl => wl.MemberID == memberId && dates.Contains(wl.Date))
                .Select(wl =>
                   new
                   {
                      wl.Date,
                       wl.WaterAmount
                   });


            for(int i = 0;i< howManyDays;i++)
            {
                string _date = dates[i];
                if (logsOfTheMonth.FirstOrDefault(wl => wl.Date == _date) != null)
                {
                    result[i] += logsOfTheMonth.FirstOrDefault(wl => wl.Date == _date).WaterAmount;
                }
            }

            return result;
        }

        public int[][] GetWeeklySuggestedWaterLogRanges(int memberId, string date) //todo upgrade
        {
            DateTime theDate = DateTime.ParseExact(date, CDictionary.MMddyyyy, CultureInfo.InvariantCulture);
            DayOfWeek dw = theDate.DayOfWeek;


            int daysToAdded = -((int)dw + 6) % 7;
            int[] WeeklySuggestedWaterLogs_Min = new int[7];
            int[] WeeklySuggestedWaterLogs_Max = new int[7];

            for (int i = 0; i < 7; i++)
            {
                string currDate = theDate.AddDays(daysToAdded).ToString(CDictionary.MMddyyyy);
                WeeklySuggestedWaterLogs_Min[i] = GetSuggestedWaterLogByDate(memberId, currDate)[0];
                WeeklySuggestedWaterLogs_Max[i] = GetSuggestedWaterLogByDate(memberId, currDate)[1];

                daysToAdded++;
            }
            int[][] WeeklySuggestedWaterLogsRanges = new int[][]{ WeeklySuggestedWaterLogs_Min, WeeklySuggestedWaterLogs_Max};
            return WeeklySuggestedWaterLogsRanges;

        }


        public int[][] GetMonthlySuggestedWaterLogRanges(int memberId, DateTime date) //todo upgrade /
        {
            int[][] ranges = new int[2][];
            double[] weights = wDao.GetMonthlyFilledWeights(memberId, date);
            
            ranges[0] = weights.Select(w => (int)Math.Round(w * 30)).ToArray();
            ranges[1] = weights.Select(w => (int)Math.Round(w * 40)).ToArray();

            return ranges;
        }


        private int[] GetSuggestedWaterLogByDate(int memberId, string date)
        {
            WeightLog latestWeightLogByDate = null;
            DateTime theDate = DateTime.ParseExact(date, CDictionary.MMddyyyy, CultureInfo.InvariantCulture);
            IQueryable<WeightLog> weightLogs = db.WeightLogs.Where(wl => wl.MemberID == memberId).OrderByDescending(wl => wl.UpdatedDate).Select(wl => wl);
            foreach (WeightLog weightLog in weightLogs)
            {
                if (weightLog.UpdatedDate <= theDate)
                {
                    latestWeightLogByDate = weightLog;
                    break; }
            }
            int[] suggestedWaterAmountRange = { (int)latestWeightLogByDate.Weight * 30, (int)latestWeightLogByDate.Weight * 40 };
            return suggestedWaterAmountRange;
        }

        public int[] GetWeeklyWaterLogs(int memberId, string date)
        {
            DateTime theDate = DateTime.ParseExact(date, CDictionary.MMddyyyy, CultureInfo.InvariantCulture);

            DayOfWeek dw = theDate.DayOfWeek;
            int daysToAdded = -((int)dw + 6) % 7;
            string[] dates = new string[7];
            int[] WeeklyWaterLogs = new int[7];
            for (int i = 0; i < 7; i++)
            {
                string currDate = theDate.AddDays(daysToAdded).ToString(CDictionary.MMddyyyy);
                dates[i] = currDate;
                daysToAdded ++;
            }
             var waterRecord = db.WaterLogs.Where(wl => wl.MemberID == memberId && dates.Contains(wl.Date)).GroupBy(wl=>wl.Date).Select(wl=> new { 
              date = wl.Key,
                  waterAmount = wl.Sum(o => o.WaterAmount)
              });
            for (int i = 0; i < 7; i++)
            {
                string _date = dates[i];
                if (waterRecord.FirstOrDefault(r => r.date == _date) != null)
                {
                    WeeklyWaterLogs[i] += waterRecord.FirstOrDefault(r => r.date == _date).waterAmount;
                }
                
            }

            return WeeklyWaterLogs;
        }


        public int GetWaterLogByDate(int memberId, string date)
        {
            int waterLog = 0;
            if (HasWaterLogByDate(memberId, date))
            {
                waterLog =db.WaterLogs.Where(wl => wl.MemberID == memberId && wl.Date == date).Select(wl => wl).Sum(wl => wl.WaterAmount);
            }
            return waterLog;
        }

       


    }
}
