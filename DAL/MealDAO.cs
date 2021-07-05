using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class MealDAO : HHContext
    {
        public IQueryable<MealOption> GetMealsByTagID(int id)
        {
            return db.MealTags.Where(mt => mt.MealTagCategoriesID == id).OrderBy(m => m.MealOption.Calories).Select(mt => mt.MealOption);

        }

        public IQueryable<MealOption> GetAllMeals()
        {
            return db.MealOptions.Select(m => m);
        }

        public IQueryable<MealOption> GetTagMealsUnderMaximumCal(int mealTagID, int maxCal)
        {


            return db.MealTags.Where(mt => mt.MealTagCategoriesID == mealTagID).Select(mt => mt.MealOption).Where(m => m.Calories <= maxCal).Select(m => m);
        }

        public dynamic GetMealsByKeyword(string mealKeyword)
        {
             return db.MealOptions.Where(m => m.Name.Contains(mealKeyword)).Select(m =>new {
                MealOptionID = m.ID,
                MealName= m.Name
            } ).ToList();
        }

        public MealOption GetMealByID(int id)
        {
            return db.MealOptions.FirstOrDefault(m => m.ID == id);
        }


        public IQueryable<MealOption> GetLikedMeals(int memberID)
        {
            return db.LikedMeals.Where(lm => lm.MemberID == memberID).Select(lm => lm.MealOption);

        }

        public bool ToggleIsLikedMeal(int memberId, int mealId)
        {
            
            var target = db.LikedMeals.FirstOrDefault(lm => lm.MemberID== memberId && lm.MealOptionID == mealId);
            if (target != null)
            {
                db.LikedMeals.Remove(target);
            }
            else 
            {
                LikedMeal entity = new LikedMeal() { MemberID = memberId, MealOptionID = mealId };
                db.LikedMeals.Add(entity);
            }
            db.SaveChanges();
            return IsLikedMeal(memberId, mealId);
        }

        public bool IsLikedMeal(int memberId, int mealId)
        {
            return db.LikedMeals.Any(lm => lm.MemberID == memberId && lm.MealOptionID == mealId);
        }
    }
}
