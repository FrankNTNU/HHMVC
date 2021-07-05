using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL;
namespace DTO
{
    public class MealOptionViewModel
    {
        MealBLL mBLL = new MealBLL();
        private MealOption _mealOption;
        private int _memberId;
        private bool _isLiked;
        public MealOptionViewModel(MealOption mealOption, int memberId)
        {
            _mealOption = mealOption;
            _memberId = memberId;
            _isLiked = mBLL.IsLikedMeal(memberId, _mealOption.ID);
        }



        public bool isLiked { get { return _isLiked; } }
        public int ID { get { return _mealOption.ID; } }
        public string Name { get { return _mealOption.Name; } }
        public double Calories { get { return _mealOption.Calories; } }
        public string Image { get { return _mealOption.Image; } }
        public string UnitName { get { return _mealOption.UnitName; } }

        public double Carbs { get { return _mealOption.Nutrient.Carbs; } }

        public double Fat { get { return _mealOption.Nutrient.Fat; } }

        public double Protein { get { return _mealOption.Nutrient.Protein; } }

        public double Na { get { return _mealOption.Nutrient.Na; } }

        public double Sugar { get { return _mealOption.Nutrient.Sugar; } }
        public bool IsLiked { get { return mBLL.IsLikedMeal(_memberId, ID); } }


    }
}
