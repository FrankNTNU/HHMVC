using DTO;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;

namespace DAL
{
    public class MealDAO : HHContext
    {
        public IQueryable<MealOption> GetMealsByTagID(int id)
        {
            if (id == 54) //ID 54為"所有餐點"
            {
                return db.MealOptions.OrderBy(m => m.Calories).Select(mt => mt);
            }
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

        public IQueryable<int> GuessMealIDsMemberWants(int memberId)
        {
            DateTime today = DateTime.Now;
            List<string> dates = new List<string>();
            for (int i = -60; i < 0; i++)
            {
                dates.Add(today.AddDays(i).ToString(CDictionary.MMddyyyy));               
            }
            var mealIds = db.DietLogs.Where(dl => dl.MemberID == memberId && dates.Contains(dl.Date)).GroupBy(dl => dl.MealOptionID).Select(dl => new
            {
                MealId = dl.Key,
                frequency = dl.Count()
            }).OrderByDescending(c => c.frequency).Take(5).Select(c=>c.MealId);


            return mealIds;

        }

        public IEnumerable<int> GetFitSuggestionMealIDs()  //todo
        {
            var winnerPrograms = db.Programs.Where(p => p.StatusID == 5).ToList();
            List<DietLog> winnerDietLogs = new List<DietLog>();
            winnerPrograms.ForEach(pg =>
            {
                winnerDietLogs.AddRange(db.DietLogs.Where(dl => dl.MemberID == pg.MemberID).Where( dl=>
                DateTime.ParseExact(dl.Date, CDictionary.MMddyyyy, CultureInfo.InvariantCulture) >= DbFunctions.TruncateTime(pg.StartDate) && DateTime.ParseExact(dl.Date, CDictionary.MMddyyyy, CultureInfo.InvariantCulture) <= DbFunctions.TruncateTime(pg.EndDate)).ToList());


            });
            var mealIds =winnerDietLogs.GroupBy(dl => dl.MealOptionID).Select(dl => new
            {
                MealOptID = dl.Key,
                frequency = dl.Count()
            }).OrderByDescending(m => m.frequency).Take(10).Select(m => m.MealOptID);

            return mealIds;
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
