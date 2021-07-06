using DAL;
using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class MealBLL
    {
        MealDAO dao = new MealDAO();

        public IQueryable<MealOption> GetMealsByTagID(int id)
        {

            return dao.GetMealsByTagID(id);
        }




        public IQueryable<MealOption> GetTagMealsUnderMaximumCal(int mealTagID, int maxCal)
        {
            return dao.GetTagMealsUnderMaximumCal(mealTagID, maxCal);
        }

        public dynamic GetMealsByKeyword(string mealKeyword)
        {

            return dao.GetMealsByKeyword(mealKeyword);
        }



        public IQueryable<MealOption> GetAllMeals()
        {

            return dao.GetAllMeals();
        }

        public MealOption GetMealByID(int id)
        {

            return dao.GetMealByID(id);
        }

        public IQueryable<MealOption> GetLikedMeals(int memberID)
        {
            return dao.GetLikedMeals(memberID);
        }

        public bool ToggleIsLikedMeal(int memberId, int mealId)
        {
            return dao.ToggleIsLikedMeal(memberId, mealId);
        }

        public bool IsLikedMeal(int memberId, int mealId)
        {
            return dao.IsLikedMeal(memberId, mealId);
        }


    }
}
