using BLL;
using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Globalization;
using DTO;

namespace UI.ViewModels
{
    public class GeneralPerformancesViewModel
    {
        private int _memberId;
        private string _date;
        WeightLogBLL wBLL = new WeightLogBLL();
        ProgramBLL pBLL = new ProgramBLL();
        DietLogBLL dlBLL = new DietLogBLL();
        WorkoutLogBLL wlBLL = new WorkoutLogBLL();
        private int _tDEE;
        private int _programMaxCalOrTDEE;
        private int _allDayGainedCal;
        int _past7daysTotalCals;
        private double _past7DaysWorkoutBurnedCals;
        private int _gainedNa;
        public GeneralPerformancesViewModel(int memberId, string date, int tDEE, int programMaxCalOrTDEE, int allDayGainedCal) 
        {
            _memberId = memberId;
            _date = date;
            _tDEE = tDEE;
            _programMaxCalOrTDEE = programMaxCalOrTDEE;
            _allDayGainedCal = allDayGainedCal;
            _past7daysTotalCals = dlBLL.Past7DaysGainedCalFromDate(_memberId, DateTime.ParseExact(_date, CDictionary.MMddyyyy, CultureInfo.InvariantCulture).AddDays(-1).ToString(CDictionary.MMddyyyy)).Sum();
            _past7DaysWorkoutBurnedCals = wlBLL.Past7DaysWorkoutBurnedCalsFromDate(_memberId, DateTime.ParseExact(_date, CDictionary.MMddyyyy, CultureInfo.InvariantCulture).AddDays(-1));
            _gainedNa = (int)Math.Round(dlBLL.GetGainedNa(_memberId, _date));
        }

        private WeightLog latestWeightByDate { get { return wBLL.GetLatestWeightByMemberIdPriorDate(_memberId, _date); } }

        private WeightLog latestWeightBy7DaysAgo { get { return wBLL.GetLatestWeightByMemberIdPriorDate(_memberId, DateTime.ParseExact(_date, CDictionary.MMddyyyy, CultureInfo.InvariantCulture).AddDays(-7).ToString(CDictionary.MMddyyyy)); } }

        public string WeightComment
        {
            get
            {
                string comment = "";
                if (latestWeightByDate.Weight > latestWeightBy7DaysAgo.Weight)
                {
                    comment = "上升";
                }
                else if (latestWeightByDate.Weight == latestWeightBy7DaysAgo.Weight)
                {
                    comment = "持平";
                }
                else
                {
                    comment = "下降";
                }
                return comment;
            }
        }

        public double WeightChange
        {
            get
            {
                return latestWeightByDate.Weight - latestWeightBy7DaysAgo.Weight;
            }
        }

        public string GainedNa
        {
            get
            {
                return _gainedNa+"mg";
            }
        }

        public double CurrentWeight
        {
            get
            {
                return latestWeightByDate.Weight ;
            }
        }

        public string ProgramNameOrDailyLog
        {
            get
            {
                string programNameOrDailyLog = "本日日常";
                Program program = pBLL.GetCurrentProgram(_memberId);
                if (program != null) { programNameOrDailyLog = program.Name; }
                return programNameOrDailyLog;
            }
        }

        public int Past7DaysWorkoutBurnedCals
        {
            get
            {
                return (int)Math.Round(_past7DaysWorkoutBurnedCals);
            }
        }

        public int CalSavedFromLastWeek
        {

            get
            {
                return _tDEE * 7 - _past7daysTotalCals;
            }
        }
        public string BurnedCalCommentFromLastWeek
        {
            get
            {
                string prefix = "消耗掉";
                double howManyBowls = (double)Math.Abs(Past7DaysWorkoutBurnedCals) / (double)CDictionary.BowlOfRiceCals;
                double Result = Math.Round(howManyBowls, 1);

                return $"{prefix}{Result}碗 ";

            }

        }
        public string CalCommentFromLastWeek
        {
            get
            {
                string prefix = CalSavedFromLastWeek > 0 ? "省下了" : "多吃了";
                double howManyBowls = (double)Math.Abs(CalSavedFromLastWeek) / (double)CDictionary.BowlOfRiceCals;
                double Result = Math.Round(howManyBowls, 1);

                return $"{prefix}{Result}碗 ";

            }

        }

        public string[] IsSuccessDay
        {
            get{
                string[] IsSuccessDay = new string[2];

                //攝取下限(基礎代謝) double DailyGainCalMinCoefficient = 0.8;
                double gainedRate = (double)_allDayGainedCal / (double)_tDEE ;

                if (gainedRate < CDictionary.DailyGainCalMinCoefficient)
                {
                    IsSuccessDay[0] = "失敗!";
                    IsSuccessDay[1] = "(攝取低於基礎代謝)";
                }
                else if (_allDayGainedCal <= _programMaxCalOrTDEE)
                {
                    IsSuccessDay[0] = "成功!";
                    IsSuccessDay[1] = "(攝取滿足挑戰上限)";
                }
                else
                {
                    IsSuccessDay[0] = "失敗!";
                    IsSuccessDay[1] = "(攝取超過挑戰上限)";
                }
                return IsSuccessDay;

            }

        }
    }
} 