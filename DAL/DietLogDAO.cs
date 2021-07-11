using DTO;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class DietLogDAO : HHContext
    {



        public void DeleteByMemberID(int ID)
        {
            List<DietLog> dietLogs = db.DietLogs.Where(x => x.MemberID == ID).ToList();
            db.DietLogs.RemoveRange(dietLogs);
            db.SaveChanges();
        }
        public void Add(DietLog entity)
        {
            db.DietLogs.Add(entity);
            db.SaveChanges();
        }


        public IQueryable<DietLog> GetDietLogsByMemberID(int memberID)
        {
            IQueryable<DietLog> q = from dl in db.DietLogs
                                    where dl.MemberID == memberID
                                    orderby dl.Date descending, dl.TimeOfDayID
                                    select dl;

            return q;
        }
     
        public List<int> GetMonthlyGainedCals(int memberId, DateTime date)
        {
            List<int> datas = new List<int>();
           
            for (int i = 1; i <= DateTime.DaysInMonth(date.Year, date.Month); i++)
            {
                string currDate = new DateTime(date.Year, date.Month, i).ToString(CDictionary.MMddyyyy);
                datas.Add((int)GetGainedCalByDate(currDate, memberId));


            }
            return datas;
        }

        public IQueryable<DietLog> GetDietLogsByDate(int memberID, string date)
        {
            return db.DietLogs.Where(dl =>dl.MemberID == memberID && dl.Date == date).OrderBy(dl => dl.TimeOfDayID).ThenByDescending(dl=>dl.MealOption.Calories).Select(dl => dl);





        }

        public IQueryable<DietLog> GetDietLogsByKeyword(string keyword, int memberID)
        {
            if (keyword == "") { return GetDietLogsByMemberID(memberID); }
            IQueryable<DietLog> q = from dl in db.DietLogs
                                    orderby dl.Date descending, dl.TimeOfDayID
                                    where dl.MealOption.Name.Contains(keyword)
                                    select dl;

            return q;
        }

        public int[] Past7DaysGainedCalFromDate( int memberID, string date) //todo need better
        {
            deBug("Past7DaysGainedCalFromDate");

            DateTime d = DateTime.Now;
                
                DateTime theDate = DateTime.ParseExact(date, CDictionary.MMddyyyy, CultureInfo.InvariantCulture);
            int[] GaineCalsPast7days = new int[7];
            for (int i = 0; i < 7; i++)
            {
                string currDay = theDate.AddDays(-i).ToString(CDictionary.MMddyyyy);

                GaineCalsPast7days[i] = (int)GetGainedCalByDate(currDay, memberID);

            }
            Array.Reverse(GaineCalsPast7days);

            d = DateTime.Now;

            return GaineCalsPast7days;

        }
        IDictionary<string, int> debugDictionary = new Dictionary<string, int>();

        private void deBug(string funcName)
        {
            if (!debugDictionary.ContainsKey(funcName))
            {
                debugDictionary[funcName] = 1;
            }
            else {
                debugDictionary[funcName] += 1;
            }
            foreach (var r in debugDictionary.Select(i => $"{i.Key}: {i.Value}").ToList()) {
                Debug.WriteLine(r);
            };
            //Debug.WriteLine(debugDictionary.ToString());
            Debug.WriteLine("\n-----------------------\n");

        }
        public int[]  GetNearby7DaysGainedCal(int memberID, string date)
        {
            deBug("GetNearby7DaysGainedCal");
            DateTime d = DateTime.Now;

            DateTime theDate = DateTime.ParseExact(date, CDictionary.MMddyyyy, CultureInfo.InvariantCulture);
            int[] Nearby7DaysGainedCals = new int[7];
            for (int i = -3; i <=3; i++)
            {
                string currDay = theDate.AddDays(i).ToString(CDictionary.MMddyyyy);
                Nearby7DaysGainedCals[i + 3] = (int)GetGainedCalByDate(currDay, memberID);


            }
            d = DateTime.Now;

            return Nearby7DaysGainedCals;

        }

        public double GetPriorAvgGainedCalTODByDate(string date, int memberId, int todId)
        {
            DateTime dd = DateTime.Now;
            deBug("GetPriorAvgGainedCalTODByDate");

            DateTime theDate = DateTime.ParseExact(date, CDictionary.MMddyyyy, CultureInfo.InvariantCulture);
            var datesFromHH = db.DietLogs.Where(dl=>dl.MemberID == memberId && dl.TimeOfDayID == todId)
                .Select(dl => dl.Date).Distinct();    //我有吃早餐的天HH
            List<string> dHH = new List<string>();
            foreach (string d in datesFromHH)
            {
                if (DateTime.ParseExact(d, CDictionary.MMddyyyy, CultureInfo.InvariantCulture) < theDate)
                {
                    dHH.Add(d);
                }
            }     //我這天之前有吃早餐的天HH

            var qH = db.DietLogs.Where(dl => dl.MemberID == memberId && dl.TimeOfDayID == todId)
                .Where(dl => dHH.Contains(dl.Date)).GroupBy(dl => dl.Date).Select(dl => new { sumCalH = dl.Sum(d => d.MealOption.Calories * d.Portion) });
            //------
            var datesFromMemberUp = db.TempCustomerMealOptions
                .Where(dl => dl.MemberID == memberId && dl.TimeOfDayID == todId && dl.StatusID != CDictionary.StatusAprove)
                .Select(dl => dl.Date).Distinct();
            List<string> dMb = new List<string>();
            foreach (string d in datesFromMemberUp)
            {
                if (DateTime.ParseExact(d, CDictionary.MMddyyyy, CultureInfo.InvariantCulture) < theDate)
                {
                    dMb.Add(d);
                }
            }
            var qM = db.TempCustomerMealOptions.Where(dl => dl.MemberID == memberId && dl.TimeOfDayID == todId &&  dl.StatusID != CDictionary.StatusAprove).Where(dl => dMb.Contains(dl.Date)).GroupBy(dl => dl.Date).Select(dl => new { sumCalM = dl.Sum(d => d.Calories * d.Portion) });

            
            var uniqDatesHM = dHH.Concat(dMb).ToHashSet();
            var a = qH.Select(q => (double?)q.sumCalH).Sum() ?? 0;
            var b = qM.Select(q=>(double?)q.sumCalM).Sum() ?? 0;
            var c = uniqDatesHM.Count;
           

            var priorTODAvg = (a + b) / c;
             dd = DateTime.Now;

            return priorTODAvg;


        }

        public double GetPriorAvgGainedCalByDate(string date, int memberId)
        {
            deBug("GetPriorAvgGainedCalByDate");
            DateTime dd = DateTime.Now;

            DateTime theDate = DateTime.ParseExact(date, CDictionary.MMddyyyy, CultureInfo.InvariantCulture);
            var datesFromHH = db.DietLogs.Where(dl => dl.MemberID == memberId )
                .Select(dl => dl.Date).Distinct();   
            List<string> dHH = new List<string>();
            foreach (string d in datesFromHH)
            {
                if (DateTime.ParseExact(d, CDictionary.MMddyyyy, CultureInfo.InvariantCulture) < theDate)
                {
                    dHH.Add(d);
                }
            }     

            var qH = db.DietLogs.Where(dl => dl.MemberID == memberId )
                .Where(dl => dHH.Contains(dl.Date)).GroupBy(dl => dl.Date).Select(dl => new { sumCalH = dl.Sum(d => d.MealOption.Calories * d.Portion) });
            //------
            var datesFromMemberUp = db.TempCustomerMealOptions
                .Where(dl => dl.MemberID == memberId  && dl.StatusID != CDictionary.StatusAprove)
                .Select(dl => dl.Date).Distinct();
            List<string> dMb = new List<string>();
            foreach (string d in datesFromMemberUp)
            {
                if (DateTime.ParseExact(d, CDictionary.MMddyyyy, CultureInfo.InvariantCulture) < theDate)
                {
                    dMb.Add(d);
                }
            }
            var qM = db.TempCustomerMealOptions.Where(dl => dl.MemberID == memberId  && dl.StatusID != CDictionary.StatusAprove).Where(dl => dMb.Contains(dl.Date)).GroupBy(dl => dl.Date).Select(dl => new { sumCalM = dl.Sum(d => d.Calories * d.Portion) });


            var uniqDatesHM = dHH.Concat(dMb).ToHashSet();
            var a = qH.Select(q => (double?)q.sumCalH).Sum() ?? 0;
            var b = qM.Select(q => (double?)q.sumCalM).Sum() ?? 0;
            var c = uniqDatesHM.Count;


            var priorTODAvg = (a + b) / c;
            dd = DateTime.Now;

            return priorTODAvg;

        }

        public void EditDietLogByID(DietLogEditDTO editDto)
        {
            if (editDto.IsCustomUploaded == false)
            {
                DietLog log = db.DietLogs.Where(dl => dl.ID == editDto.DietLogID).FirstOrDefault();
                if (log != null)
                {
                    log.Portion = editDto.Portion;
                    log.TimeOfDayID = editDto.TimeOfDayID;
                    db.SaveChanges();
                }
            }
            else {
                TempCustomerMealOption log = db.TempCustomerMealOptions.Where(tcm => tcm.ID == editDto.DietLogID).FirstOrDefault();
                if (log != null)
                {
                    log.Portion = editDto.Portion;
                    log.TimeOfDayID = editDto.TimeOfDayID;
                    db.SaveChanges();
                }


            }
           

        }

        public double GetGainedProtein(int memberID, string date)
        {
            deBug("GetGainedProtein");

            DateTime d = DateTime.Now;

            if (HasHHDietLogsByDate(memberID, date))
            {
                return db.DietLogs.Where(dl => dl.MemberID == memberID && dl.Date == date).Select(dl => dl).Sum(dl => dl.MealOption.Nutrient.Protein * dl.Portion);
            }
             d = DateTime.Now;

            return 0;

        }

        public double GetGainedCarbs(int memberID, string date)
        {
            deBug("GetGainedCarbs");

            DateTime d = DateTime.Now;

            if (HasHHDietLogsByDate(memberID, date))
            {
                return db.DietLogs.Where(dl => dl.MemberID == memberID && dl.Date == date).Select(dl => dl).Sum(dl => dl.MealOption.Nutrient.Carbs * dl.Portion);
            }
             d = DateTime.Now;

            return 0;

        }

        public double GetGainedSugar(int memberID, string date)
        {
            deBug("GetGainedCarbs");

            DateTime d = DateTime.Now;

            if (HasHHDietLogsByDate(memberID, date))
            {
                return db.DietLogs.Where(dl => dl.MemberID == memberID && dl.Date == date).Select(dl => dl).Sum(dl => dl.MealOption.Nutrient.Sugar * dl.Portion);
            }
             d = DateTime.Now;

            return 0;
        }

        public double GetGainedFat(int memberID, string date)
        {
            deBug("GetGainedFat");
            DateTime d = DateTime.Now;

            if (HasHHDietLogsByDate(memberID, date))
            {
                return db.DietLogs.Where(dl => dl.MemberID == memberID && dl.Date == date).Select(dl => dl).Sum(dl => dl.MealOption.Nutrient.Fat * dl.Portion);
            }
             d = DateTime.Now;

            return 0;
        }

        public double GetGainedNa(int memberID, string date)
        {
            deBug("GetGainedNa");

            DateTime d = DateTime.Now;

            if (HasHHDietLogsByDate(memberID, date))
            {
                return db.DietLogs.Where(dl => dl.MemberID == memberID && dl.Date == date).Select(dl => dl).Sum(dl => dl.MealOption.Nutrient.Na * dl.Portion);
            }
             d = DateTime.Now;

            return 0;
        }

        public double GetGainedCalByDateTimeOfDay(int memberID, string date, int timeOfDayID)   
        {
            deBug("GetGainedCalByDateTimeOfDay");

            DateTime d = DateTime.Now;

            double fromHHMealOptsGainedTODCal = 0;
            double fromCustomUploadedGainedTODCal = 0;


            if (HasHHDietLogsByDateTOD(memberID, date, timeOfDayID))
            {
                fromHHMealOptsGainedTODCal = db.DietLogs.Where(dl => dl.MemberID == memberID && dl.Date == date && dl.TimeOfDayID == timeOfDayID)
                .Select(dl => dl).Sum(dl => dl.MealOption.Calories * dl.Portion);
            }

            if (HasCustomUploadedNonApprovedMealsByDateTOD(memberID, date, timeOfDayID))
            {
                fromCustomUploadedGainedTODCal = db.TempCustomerMealOptions
                    .Where(tcm => tcm.MemberID == memberID && tcm.Date == date && tcm.TimeOfDayID == timeOfDayID && tcm.StatusID != CDictionary.StatusAprove)
                    .Select(dl => dl).Sum(tcm => tcm.Calories * tcm.Portion);
            }
             d = DateTime.Now;

            return Math.Round(fromHHMealOptsGainedTODCal + fromCustomUploadedGainedTODCal);
        }

        public bool HasCustomUploadedNonApprovedMealsByDate(int memberID, string date)
        {            deBug("HasCustomUploadedNonApprovedMealsByDate");

            return db.TempCustomerMealOptions.Any(tcm => tcm.MemberID == memberID && tcm.Date == date && tcm.StatusID !=CDictionary.StatusAprove);
        }

        public bool HasCustomUploadedNonApprovedMealsByDateTOD(int memberID, string date, int todId)
        {            deBug("HasCustomUploadedNonApprovedMealsByDateTOD");

            return db.TempCustomerMealOptions
                .Any(tcm => tcm.MemberID == memberID && tcm.Date == date && tcm.TimeOfDayID == todId && tcm.StatusID != CDictionary.StatusAprove);
        }

        public bool HasHHDietLogsByDate( int memberID, string date)
        {           
            deBug("HasHHDietLogsByDate");

            return db.DietLogs.Where(dl=>dl.MemberID == memberID).Any(dl => dl.Date == date);
        }

        public bool HasHHDietLogsByDateTOD(int memberID, string date, int todId)
        {
            deBug("HasHHDietLogsByDateTOD");

            return db.DietLogs.Where(dl => dl.MemberID == memberID && dl.Date == date).Any(dl => dl.TimeOfDayID == todId);
        }

        public double GetGainedCalByDate(string date, int memberID)
        {
            deBug("GetGainedCalByDate");

            DateTime d = DateTime.Now;

            double fromHHMealOptsGainedCal = 0;
            double fromCustomUploadedGainedCal = 0;

            if (HasHHDietLogsByDate(memberID, date ))
            {
                fromHHMealOptsGainedCal = db.DietLogs.Where(dl => dl.MemberID == memberID && dl.Date == date)
                .Select(dl => dl).Sum(dl => dl.MealOption.Calories * dl.Portion);
            }

            if (HasCustomUploadedNonApprovedMealsByDate(memberID, date))
            {
                fromCustomUploadedGainedCal = db.TempCustomerMealOptions
                    .Where(tcm => tcm.MemberID == memberID && tcm.Date == date && tcm.StatusID != CDictionary.StatusAprove)
                    .Select(dl => dl).Sum(tcm => tcm.Calories * tcm.Portion);
            }
             d = DateTime.Now;

            return Math.Round(fromHHMealOptsGainedCal+ fromCustomUploadedGainedCal);




        }

        public void DeleteDietLog(int id, bool IsCustomUploaded)
        {
            if (!IsCustomUploaded)
            {
                DietLog theDl = db.DietLogs.FirstOrDefault(dl => dl.ID == id);
                if (theDl != null)
                {
                    db.DietLogs.Remove(theDl);
                    db.SaveChanges();
                }
            }
            else
            {
                TempCustomerMealOption theDl = db.TempCustomerMealOptions.FirstOrDefault(tcm => tcm.ID == id);
                if (theDl != null)
                {
                    db.TempCustomerMealOptions.Remove(theDl);
                    db.SaveChanges();
                }
            }
        }
        public void AddToDietLog(DietLog dietLog)
        {
            try
            {
                db.DietLogs.Add(dietLog);
                db.SaveChanges();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
