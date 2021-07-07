using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;


namespace DAL
{
    public class WeightLogDAO : HHContext
    {
        public void DeleteByMemberID(int ID)
        {
            List<WeightLog> weightLogs = db.WeightLogs.Where(x => x.MemberID == ID).ToList();
            db.WeightLogs.RemoveRange(weightLogs);
            db.SaveChanges();
        }

        public WeightLog GetLatestWeightByMemberID(int memberId)
        {
            return db.WeightLogs.Where(wl => wl.MemberID == memberId).OrderByDescending(wl => wl.UpdatedDate).FirstOrDefault();
        }

        public WeightLog GetLatestWeightByMemberIdPriorDate(int memberId, string date)
        {
            DateTime theDate = DateTime.ParseExact(date, CDictionary.MMddyyyy, CultureInfo.InvariantCulture);
            WeightLog theRecord = null;
             var weightLogs = db.WeightLogs.Where(wl => wl.MemberID == memberId).OrderByDescending(wl => wl.UpdatedDate).ToList();
            for (int i = 0; i < weightLogs.Count; i++)
            {
                if (weightLogs[i].UpdatedDate<= theDate)
                {
                    theRecord = weightLogs[i];
                    break;
                }
            }
            return theRecord;

        }

        public double[] GetMonthlyWeights(int memberId, DateTime date)
        {
            int howManyDays = DateTime.DaysInMonth(date.Year, date.Month);
            double[] weights = new double[howManyDays];
            var WeightsOfTheMonth = db.WeightLogs
               .Where(wl => wl.MemberID == memberId && wl.UpdatedDate.Month == date.Month && wl.UpdatedDate.Year == date.Year)
               .OrderBy(wl => wl.UpdatedDate).Select(wl =>
                new {
                    day = wl.UpdatedDate.Day,
                    weight = wl.Weight
                });
            foreach (var weight in WeightsOfTheMonth)
            {
                weights[weight.day - 1] = weight.weight;
            }

            return weights;
        }

        public void AddWeightLogViaProgramRegister(int memberID, int weight)
        {
            //if (db.WeightLogs.FirstOrDefault(wl => wl.MemberID == memberID && wl.UpdatedDate.ToString(CDictionary.MMddyyyy) == DateTime.Now.ToString(CDictionary.MMddyyyy)) != null)
            //{
            //    int k = 0;
            //}
            //else { 
            //}
        }

        public void AddWeightLog(WeightLog weightLog)
        {
            db.WeightLogs.Add(weightLog);
            db.SaveChanges();
        }
    }
}
