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

        public double[] GetMonthlyRecordedWeights(int memberId, DateTime date)
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

        public double[] GetMonthlyFilledWeights(int memberId, DateTime date)
        {
            int howManyDays = DateTime.DaysInMonth(date.Year, date.Month);

            double[] weights = new double[howManyDays];
            var WeightsOfTheMonth = db.WeightLogs
               .Where(wl => wl.MemberID == memberId && wl.UpdatedDate.Month == date.Month && wl.UpdatedDate.Year == date.Year)
               .OrderBy(wl => wl.UpdatedDate).Select(wl =>
                new
                {
                    day = wl.UpdatedDate.Day,
                    weight = wl.Weight
                });
            foreach (var weight in WeightsOfTheMonth)
            {
                weights[weight.day - 1] = weight.weight;
            }
            if (weights[0] == 0)
            {
                DateTime firstDateOfTheMonth = new DateTime(date.Year, date.Month, 1);
                weights[0] = db.WeightLogs
                  .Where(wl => wl.MemberID == memberId && DbFunctions.TruncateTime(wl.UpdatedDate) < firstDateOfTheMonth)
                  .OrderByDescending(wl => wl.UpdatedDate).FirstOrDefault().Weight;
            }
            for (int i = 0; i < howManyDays; i++)
            {
                if (weights[i] == 0)
                {
                    for (int k = i; k >= 0; k--)
                    {
                        if (weights[k] > 0)
                        {
                            weights[i] = weights[k];
                            break;
                        }
                    }
                }
            }

            return weights;
        }

        public void AddWeightLogViaProgramRegister(int memberID, int weight)
        {
            var weightLogOfToday = db.WeightLogs.FirstOrDefault(wl => wl.MemberID == memberID && DbFunctions.TruncateTime(wl.UpdatedDate) == DateTime.Today);
            if (weightLogOfToday != null)
            {
                weightLogOfToday.Weight = weight;
                weightLogOfToday.UpdatedDate = DateTime.Now;

                db.SaveChanges();
            }

            
            else
            {
                WeightLog entity = new WeightLog()
                {
                    MemberID = memberID,
                    Weight = (double)weight,
                    UpdatedDate = DateTime.Now

                };
                db.WeightLogs.Add(entity);
                db.SaveChanges();
            }
        }
    }
}
