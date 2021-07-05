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
    public class GainedCaloriesViewModel
    {
        DietLogBLL dlBLL = new DietLogBLL();
        private int _memberId;
        private string _date;
        private int _programMaxOrTDEE;
        private int[] _tODPercents;
        private int[] _tODGaineds;
        private int[] _TODGainedGrowthRates;
        private int[] _tODPriorAvgPercents;
        private double _allDayGained;

        private double _breakfastGained;
        private double _lunchGained;
        private double _snackGained;
        private double _dinnerGained;
        private double _bedtimeSnackGained;


        private double _priorAvgAllDayGained;
        private double _breakfastGainedPriorAvg;
        private double _lunchGainedPriorAvg;
        private double _snackGainedPriorAvg;
        private double _dinnerGainedPriorAvg;
        private double _bedtimeSnackGainedPriorAvg;

        //GetGainedCalByDateTimeOfDay: 50
        //HasHHDietLogsByDateTOD: 50
        //HasCustomUploadedNonApprovedMealsByDateTOD: 50



        public GainedCaloriesViewModel(int memberId, string date, int programMaxOrTDEE, double gainedCalTheDate)
        {
            _memberId = memberId;
            _date = date;
            _programMaxOrTDEE = programMaxOrTDEE;
            _tODPercents = new int[5];
            _tODGaineds = new int[5];
            _TODGainedGrowthRates = new int[5];
            _tODPriorAvgPercents = new int[5];

            _allDayGained = gainedCalTheDate;
            _breakfastGained = dlBLL.GetGainedCalByDateTimeOfDay(_memberId, _date, CDictionary.Breakfast);
            _lunchGained = dlBLL.GetGainedCalByDateTimeOfDay(_memberId, _date, CDictionary.Lunch);
            _snackGained = dlBLL.GetGainedCalByDateTimeOfDay(_memberId, _date, CDictionary.Snack);
            _dinnerGained = dlBLL.GetGainedCalByDateTimeOfDay(_memberId, _date, CDictionary.Dinner);
            _bedtimeSnackGained = dlBLL.GetGainedCalByDateTimeOfDay(_memberId, _date, CDictionary.BedtimeSnack);

            _priorAvgAllDayGained = dlBLL.GetPriorAvgGainedCalByDate(_date, _memberId);
            _breakfastGainedPriorAvg = dlBLL.GetPriorAvgGainedCalTODByDate(_date, _memberId, CDictionary.Breakfast);
            _lunchGainedPriorAvg = dlBLL.GetPriorAvgGainedCalTODByDate(_date, _memberId, CDictionary.Lunch);
            _snackGainedPriorAvg = dlBLL.GetPriorAvgGainedCalTODByDate(_date, _memberId, CDictionary.Snack);
            _dinnerGainedPriorAvg = dlBLL.GetPriorAvgGainedCalTODByDate(_date, _memberId, CDictionary.Dinner);
            _bedtimeSnackGainedPriorAvg = dlBLL.GetPriorAvgGainedCalTODByDate(_date, _memberId, CDictionary.BedtimeSnack);

        }





        public int AllDayGained { get { return (int)_allDayGained; } }

        public int BreakfastGained { get { return (int)_breakfastGained; } }
        public int LunchGained { get { return (int)_lunchGained; } }
        public int SnackGained { get { return (int)_snackGained; } }
        public int DinnerGained { get { return (int)_dinnerGained; } }
        public int BedtimeSnackGained { get { return (int)_bedtimeSnackGained; } }




        public int AllDayGainedGrowthRate { get { return _priorAvgAllDayGained > 0 ? (int)((AllDayGained - _priorAvgAllDayGained) / _priorAvgAllDayGained * 100) : 0; } }
        private int BreakfastGainedGrowthRate { get { return _breakfastGainedPriorAvg > 0 ? (int)((_breakfastGained - _breakfastGainedPriorAvg) / _breakfastGainedPriorAvg * 100) : 0; } }
        private int LunchGainedGrowthRate { get { return _lunchGainedPriorAvg > 0 ? (int)((_lunchGained - _lunchGainedPriorAvg) / _lunchGainedPriorAvg * 100) : 0; } }
        private int SnackGainedGrowthRate { get { return _snackGainedPriorAvg > 0 ? (int)((_snackGained - _snackGainedPriorAvg) / _snackGainedPriorAvg * 100) : 0; } }
        private int DinnerGainedGrowthRate { get { return _dinnerGainedPriorAvg > 0 ? (int)((_dinnerGained - _dinnerGainedPriorAvg) / _dinnerGainedPriorAvg * 100) : 0; } }
        private int BedtimeSnackGainedGrowthRate { get { return _bedtimeSnackGainedPriorAvg > 0 ? (int)((_bedtimeSnackGained - _bedtimeSnackGainedPriorAvg) / _bedtimeSnackGainedPriorAvg * 100) : 0; } }














        public int AllDayGainedPercent { get { return CCalculator.GainedCalPercentage(_allDayGained, _programMaxOrTDEE); } }
        public int BreakfastGainedPercent { get { return CCalculator.GainedCalPercentage(_breakfastGained, _programMaxOrTDEE); } }
        public int LunchGainedPercent { get { return CCalculator.GainedCalPercentage(_lunchGained, _programMaxOrTDEE); } }
        public int SnackGainedPercent { get { return CCalculator.GainedCalPercentage(_snackGained, _programMaxOrTDEE); } }
        public int DinnerGainedPercent { get { return CCalculator.GainedCalPercentage(_dinnerGained, _programMaxOrTDEE); } }
        public int BedtimeSnackGainedPercent { get { return CCalculator.GainedCalPercentage(_bedtimeSnackGained, _programMaxOrTDEE); } }



public int[] TODPriorAvgPercents
        {
            get
            {
                _tODPriorAvgPercents[0] = (int)Math.Round((_breakfastGainedPriorAvg / _priorAvgAllDayGained) *100);
                _tODPriorAvgPercents[1] = (int)Math.Round((_lunchGainedPriorAvg / _priorAvgAllDayGained) * 100);
                _tODPriorAvgPercents[2] = (int)Math.Round((_snackGainedPriorAvg / _priorAvgAllDayGained) * 100);
                _tODPriorAvgPercents[3] = (int)Math.Round((_dinnerGainedPriorAvg / _priorAvgAllDayGained) * 100);
                _tODPriorAvgPercents[4] = (int)Math.Round((_bedtimeSnackGainedPriorAvg / _priorAvgAllDayGained) * 100);

                return _tODPriorAvgPercents;
            }
        }  //%

        public int[] TODPercents
        {
            get
            {             
                _tODPercents[0] = BreakfastGainedPercent;
                _tODPercents[1] = LunchGainedPercent;
                _tODPercents[2] = SnackGainedPercent;
                _tODPercents[3] = DinnerGainedPercent;
                _tODPercents[4] = BedtimeSnackGainedPercent;

                return _tODPercents;
            }
        }  //%

        public int[] TODGaineds
        {
            get
            {
                _tODGaineds[0] = BreakfastGained;
                _tODGaineds[1] = LunchGained;
                _tODGaineds[2] = SnackGained;
                _tODGaineds[3] = DinnerGained;
                _tODGaineds[4] = BedtimeSnackGained;

                return _tODGaineds;
            }
        }   //kCal

        public int[] TODGainedGrowthRates   //%
        {
            get
            {
                _TODGainedGrowthRates[0] = BreakfastGainedGrowthRate;
                _TODGainedGrowthRates[1] = LunchGainedGrowthRate;
                _TODGainedGrowthRates[2] = SnackGainedGrowthRate;
                _TODGainedGrowthRates[3] = DinnerGainedGrowthRate;
                _TODGainedGrowthRates[4] = BedtimeSnackGainedGrowthRate;

                return _TODGainedGrowthRates;
            }
        }








    }
}