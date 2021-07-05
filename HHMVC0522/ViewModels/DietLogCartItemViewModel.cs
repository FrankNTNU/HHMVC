using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;

namespace DTO.ViewModels
{
    public class DietLogCartItemViewModel
    {
        private MealOption _Meal;
        public DietLogCartItemViewModel(MealOption Meal)
        {
            _Meal = Meal;
        }
        public string AddedTime { get; set; }
        //public MealOption Meal { get; set; } 

        public double Portion { get; set; }

        public string Date { get; set; }   /*MMddyyyy(this format)*/

        //public IQueryable<TimesOfDay> TimeOfDay { get; set; }  //todo

        public int TimeOfDayID { get; set; }  //todo



        public double TotalGainedCalOfMeal { get { return _Meal.Calories * Portion; } }

        public int MealOptID { get { return _Meal.ID; } }

        public string MealName { get { return _Meal.Name; } }

        public string MealImagePath { get { return _Meal.Image; } }
        public double MealCal { get { return _Meal.Calories; } }
        public string MealUnitName { get { return _Meal.UnitName; } }

        public bool IsValid { get; set; }









    }
}
