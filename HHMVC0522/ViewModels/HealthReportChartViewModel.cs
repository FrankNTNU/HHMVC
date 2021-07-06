using BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UI.ViewModels
{
    public class HealthReportChartViewModel
    {
        private int _memberId;
        private DateTime _date;
        DietLogBLL dlBLL = new DietLogBLL();
        WorkoutLogBLL woBLL = new WorkoutLogBLL();
        WeightLogBLL wtBLL = new WeightLogBLL();
        public HealthReportChartViewModel(int memberId, DateTime date)
        {
            _memberId = memberId;
            _date = date;
        }

        public int  MemberID {get{ return _memberId; } }
        public string YearMonth { get {
                return $"{_date.Year}/{_date.Month}";
            } }
        public List<int> GainedCalsByMonth{ get { return dlBLL.GetMonthlyGainedCals(_memberId, _date); } }
        public List<int> MonthlyDays
        {
            get
            {
                List<int> days = new List<int>();
                for (int i = 1; i <= DateTime.DaysInMonth(_date.Year, _date.Month); i++)
                {
                    days.Add(i);
                }
                return days;
            }
        }

        public int[] MonthlyBurnedCals
        {
            get
            {

               
                return woBLL.GetMonthlyBurnedCals(_memberId, _date).Select(d => (int)Math.Round(d)).ToArray();
            }
        }

        public List<int> MonthlySubstratedGained
        {
            get
            {
                List<int> substractedGaineds = GainedCalsByMonth;
                for (int i = 0; i < substractedGaineds.Count; i++)
                {
                    substractedGaineds[i] = substractedGaineds[i] - MonthlyBurnedCals[i];
                }


                return substractedGaineds;
            }
        }

        public double[] MonthlyWeights
        {
            get
            {
                return wtBLL.GetMonthlyWeights(_memberId, _date);
            }
        }
        




    }
}