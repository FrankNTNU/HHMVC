using BLL;
using DAL;
using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using UI.Models;

namespace UI.ViewModels
{
    public class HealthReportChartViewModel
    {
        private int _memberId;
        private DateTime _date;
        private double[] _monthlyWeights;
        DietLogBLL dlBLL = new DietLogBLL();
        WorkoutLogBLL woBLL = new WorkoutLogBLL();
        WeightLogBLL wtBLL = new WeightLogBLL();
        WaterLogBLL wBLL = new WaterLogBLL();
        MemberBLL mbBLL = new MemberBLL();
        PointBLL ptBLL = new BLL.PointBLL();
        public HealthReportChartViewModel(int memberId, DateTime date)
        {
            _memberId = memberId;
            _date = date;

            _monthlyWeights = wtBLL.GetMonthlyFilledWeights(_memberId, _date);
        }
        public bool[] MonthlyIsSuccess { get { return ptBLL.GetMonthlyIsSuccess(_memberId,_date); } }

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
        public int[] MonthlyTDEEs
        {
            get
            {
                int[] results = new int[DateTime.DaysInMonth(_date.Year, _date.Month)];
                for (int i = 0; i < results.Length; i++)
                {
                    string theDate = new DateTime(_date.Year, _date.Month, i + 1).ToString(CDictionary.MMddyyyy);
                    Member member = mbBLL.GetMemberByMemberID(_memberId);
                    MemberForDietDTO memberDto = new MemberForDietDTO(theDate)
                    {
                        MemberID = member.ID,
                        Birthdate = member.Birthdate,
                        ActivityLevelID = member.ActivityLevelID,
                        Gender = member.Gender,
                        Height = member.Height,

                    };
                    results[i] = (int)Math.Round(HealthCalculator.TDEE(memberDto, memberDto.Age, (decimal)MonthlyWeights[i]));

                }
                return results;
            }
        }

        public int[] MonthlyGainedWater  
        {
            get
            {


                return wBLL.GetMonthlyGainedWater(_memberId, _date);
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
                return _monthlyWeights;
            }
        }

        public int[][] MonthlySuggestedWaterLogRanges
        {
            get
            {
                return wBLL.GetMonthlySuggestedWaterLogRanges(_memberId, _date);
            }
        }




    }
}