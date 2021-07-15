using BLL;
using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using UI.ViewModels;

namespace UI.Models
{
    public class ViewModelGenerator
    {
        DietLogBLL dlBLL = new DietLogBLL();
        CustomerMealBLL cmBLL = new CustomerMealBLL();

        public List<DietLogViewModel> GetDietLogsByDate(int memberId, string date)
        {
            List<DietLogViewModel> dietLogVModels = new List<DietLogViewModel>();

            foreach (DietLog dl in dlBLL.GetDietLogsByDate(memberId, date))
            {
                dietLogVModels.Add(new DietLogViewModel(dl));
            }

            foreach (TempCustomerMealOption dl in cmBLL.GetCustomerMealByDate(memberId, date))
            {
                DietLogViewModel model = new DietLogViewModel(dl);
                dietLogVModels.Add(model);
            }
            dietLogVModels.OrderBy(vm => vm.TimesOfDayID).ThenBy(vm => vm.MealTotalGainedCal);
            return dietLogVModels;
        }

    }
}