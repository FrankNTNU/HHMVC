using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
namespace UI.ViewModels
{
    public class DietLogViewModel
    {
        private DietLog _DietLog;
        private TempCustomerMealOption _TempCustomerMealOption;

        public DietLogViewModel(DietLog dl) {
            _DietLog = dl;
        }

        public DietLogViewModel(TempCustomerMealOption cm)
        {
            _TempCustomerMealOption = cm;
        }

        public int ID
        {
            get {return IsCustomUploaded? _TempCustomerMealOption.ID: _DietLog.ID  ; }
        }

        public bool IsCustomUploaded
        {
            get { return _TempCustomerMealOption != null; }

        }



        public double Portion { get { return IsCustomUploaded ?  _TempCustomerMealOption.Portion: _DietLog.Portion; } }
        public int MealOptionID { get { return IsCustomUploaded ? -1: _DietLog.MealOptionID; } }
        public string Date { get { return IsCustomUploaded ? _TempCustomerMealOption.Date: _DietLog.Date; } }
        public bool IsValid { get { return IsCustomUploaded ? _TempCustomerMealOption.IsValid: _DietLog.IsValid; } }

        public  string MealName { get { return IsCustomUploaded ? _TempCustomerMealOption.Name : _DietLog.MealOption.Name; } }
        //public  Member Member { get; set; }
        public string TimesOfDayName { get { return IsCustomUploaded ? _TempCustomerMealOption.TimesOfDay.Name : _DietLog.TimesOfDay.Name; } }

        public int TimesOfDayID { get { return IsCustomUploaded ? _TempCustomerMealOption.TimeOfDayID : _DietLog.TimeOfDayID; } }
        public double MealCal { get { return IsCustomUploaded ? _TempCustomerMealOption.Calories:_DietLog.MealOption.Calories; } }

        public string MealImage { get { return IsCustomUploaded ? _TempCustomerMealOption.ImagePath : _DietLog.MealOption.Image; } }

        public string MealUnitName { get { return IsCustomUploaded ? _TempCustomerMealOption.UnitName : _DietLog.MealOption.UnitName; } }

        public double MealTotalGainedCal { get { return MealCal * Portion;  } }

    }
}