using BLL;
using DAL;
using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UI.ViewModels
{
    public class MealStyleViewModel
    {
        MealBLL moBLL = new MealBLL();
        MealTagBLL mtBLL = new MealTagBLL();
        private MealTagCategory _mealTag;
        private List<MealOptionViewModel> _mealOptViewModels;
        public MealStyleViewModel(int mealStyleId, int memberId)
        {
            _mealTag = mtBLL.GetTagByID(mealStyleId);
            _mealOptViewModels = new List<MealOptionViewModel>();
            foreach (MealOption m in moBLL.GetMealsByTagID(mealStyleId))
            {
                MealOptionViewModel model = new MealOptionViewModel(m, memberId);
                _mealOptViewModels.Add(model);
            }
        }
        public List<MealOptionViewModel> Meals { get { return _mealOptViewModels; } }

        public string MealStyleName {get{ return _mealTag.Name;   } }
        public int MealStyleID{ get { return _mealTag.ID; } }

    }
}