using BLL;
using DAL;
using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using UI.ViewModels;
using System.Globalization;


namespace UI.Models
{
    public class MemberHealthProfile
    {
        WeightLogBLL wBLL = new WeightLogBLL();
        DietLogBLL dlBLL = new DietLogBLL();
        ProgramBLL pBLL = new ProgramBLL();
        private string _date;
        private MemberForDietDTO _memberForDiet;
        private double _gainedCalTheDate;


        private GainedCaloriesViewModel _gainedCalDatas;
        private GainedNutritionViewModel _gainedNutritionDatas;
        private WaterLogDatasViewModel _waterLogDatasViewModel;
        private GeneralPerformancesViewModel _generalPerformanceViewModel;
        private bool _needGeneralReport;

        public MemberHealthProfile(MemberForDietDTO mDto, string date ,bool needGeneralReport) {
            _date = date;
            _memberForDiet = mDto;
            _needGeneralReport = needGeneralReport;
            _gainedCalTheDate = dlBLL.GetGainedCalByDate(date, mDto.MemberID);



            _gainedCalDatas = new GainedCaloriesViewModel(_memberForDiet.MemberID, _date, ProgramMaxCalOrTDEE, _gainedCalTheDate);
       
            _gainedNutritionDatas = new GainedNutritionViewModel(_memberForDiet.MemberID, _date, _gainedCalTheDate);
            _waterLogDatasViewModel = new WaterLogDatasViewModel(_memberForDiet.MemberID, _date);
            _generalPerformanceViewModel = new GeneralPerformancesViewModel(_memberForDiet.MemberID, _date, TDEE, ProgramMaxCalOrTDEE, GainedCalDatas.AllDayGained);
        }


        public int MemberID { get { return _memberForDiet.MemberID; } }
        public DateTime Birthdate { get { return _memberForDiet.Birthdate; } }

        public int ActivityLevelID { get { return _memberForDiet.ActivityLevelID; } }

        public bool Gender { get { return _memberForDiet.Gender; } }

        public double Height { get { return _memberForDiet.Height; } }



        
        public decimal Weight { get { return (decimal)wBLL.GetLatestWeightByMemberIdPriorDate(MemberID, _date).Weight; } }  

        public ProgramDTO CurrProgram { get { return _memberForDiet.Program; } }

        public int TDEE { get { return (int)Math.Round(HealthCalculator.TDEE(_memberForDiet, _memberForDiet.Age, Weight)); } }  //TODO coordinate with Enchi
        public int ProgramMaxCalOrTDEE
        {
            get
            {

                if (CurrProgram != null)
                {
                    return (int)HealthCalculator.GetProgramMaxCal(_memberForDiet,CurrProgram, _memberForDiet.Age);
                }
               
                return TDEE;
            }
        }  

        public GainedCaloriesViewModel GainedCalDatas { get { return _gainedCalDatas;  } }   

        public int[] Past7DaysGainedCalFromDate { get { return dlBLL.Past7DaysGainedCalFromDate(MemberID, _date); } }

        public int[] GetNearby7DaysGainedCal { get { return dlBLL.GetNearby7DaysGainedCal(MemberID, _date); } }

        public GainedNutritionViewModel GainedNutritionDatas { get { return _gainedNutritionDatas; } }  

        public WaterLogDatasViewModel WaterLogDatas { get { return _waterLogDatasViewModel; } }

        public GeneralPerformancesViewModel GeneralPerformances { get { return _needGeneralReport? _generalPerformanceViewModel: null; }}





    }
}