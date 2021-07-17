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
        public NutrientDTO GetNutrient(int ID)
        {
            NutrientDAO dao = new NutrientDAO();
            MealDetailDTO dto = new MealDetailDTO();
            dto.Nutrient = dao.GetNutrient(ID);
            return dto.Nutrient;
        }
        public TagCategoryDetailDTO GetTagCategory(int ID)
        {
            MealTagDAO dao = new MealTagDAO();
            MealDetailDTO dto = new MealDetailDTO();
            dto.TagCategoryDetail = dao.GetTags(ID);
            return dto.TagCategoryDetail;
        }
        public IQueryable<MealOption> GetMealsByTagID(int id)
        {
            if (id == 54) //ID 54為"所有餐點"
            {
                return db.MealOptions.OrderBy(m => m.Calories).Select(mt => mt);
            }
            return db.MealTags.Where(mt => mt.MealTagCategoriesID == id).OrderBy(m => m.MealOption.Calories).Select(mt => mt.MealOption);
        }

        public List<TagCategoryDetailDTO> GetTagCategoryList(int ID)
        {
            MealTagDAO dao = new MealTagDAO();
            MealDetailDTO dto = new MealDetailDTO();
            dto.TagList = dao.GetTagsList(ID);
            return dto.TagList;
        }
        public List<MealDetailDTO> GetMeals()
        {
            var list = db.MealOptions.ToList();

            List<MealDetailDTO> dtoList = new List<MealDetailDTO>();
            foreach (var item in list)
            {
                MealDetailDTO dto = new MealDetailDTO();
                dto.ID = item.ID;
                dto.Name = item.Name;
                dto.Calories = Convert.ToInt32(item.Calories);
                dto.MealOptionImage = item.Image;
                dto.UnitName = item.UnitName;
                dto.Nutrient = GetNutrient(dto.ID);
                dto.NutrientID = dto.Nutrient.ID;
                dto.Fat = dto.Nutrient.Fat;
                dto.Protein = dto.Nutrient.Protein;
                dto.Carbs = dto.Nutrient.Carbs;
                dto.Sugar = dto.Nutrient.Sugar;
                //dto.VitA = dto.Nutrient.VitA;
                //dto.VitB = dto.Nutrient.VitB;
                //dto.VitC = dto.Nutrient.VitC;
                //dto.VitD = dto.Nutrient.VitD;
                //dto.VitE = dto.Nutrient.VitE;
                dto.Na = dto.Nutrient.Na;
                //dto.Potassium = dto.Nutrient.Potassium;

                List<TagCategoryDetailDTO> TagList = new List<TagCategoryDetailDTO>();
                TagList = GetTagCategoryList(dto.ID);
                foreach(var tagstring in TagList)
                {
                    dto.TagStringList += tagstring.Name;
                    dto.TagStringList +="/";
                }
                dtoList.Add(dto);
            }
            return dtoList;
        }
        public List<MealDetailDTO> GetNutrientAndMeals()
        {
            var list = db.MealOptions.ToList();

            List<MealDetailDTO> dtoList = new List<MealDetailDTO>();
            foreach (var item in list)
            {
                MealDetailDTO dto = new MealDetailDTO();
                dto.ID = item.ID;
                dto.Name = item.Name;
                dto.Calories = Convert.ToInt32(item.Calories);
                dto.MealOptionImage = item.Image;
                dto.UnitName = item.UnitName;
                dto.Nutrient = GetNutrient(dto.ID);
                dto.NutrientID = dto.Nutrient.ID;
                dto.Fat = dto.Nutrient.Fat;
                dto.Protein = dto.Nutrient.Protein;
                dto.Carbs = dto.Nutrient.Carbs;
                dto.Sugar = dto.Nutrient.Sugar;
                dto.VitA = dto.Nutrient.VitA;
                dto.VitB = dto.Nutrient.VitB;
                dto.VitC = dto.Nutrient.VitC;
                dto.VitD = dto.Nutrient.VitD;
                dto.VitE = dto.Nutrient.VitE;
                dto.Na = dto.Nutrient.Na;
                dto.Potassium = dto.Nutrient.Potassium;

                dtoList.Add(dto);
            }
            return dtoList;
        }
        public IQueryable<MealOption> GetAllMeals()
        {
            return db.MealOptions.Select(m => m);
        }


        public MealDetailDTO GetMeals(int ID)
        {
            using (HealthHelperEntities db = new HealthHelperEntities())
            {
                MealOption mealOption = db.MealOptions.First(x => x.ID == ID);
                MealDetailDTO dto = new MealDetailDTO();
                dto.ID = mealOption.ID;
                dto.Name = mealOption.Name;
                dto.Calories = Convert.ToInt32(mealOption.Calories);
                dto.MealOptionImage = mealOption.Image;
                dto.UnitName = mealOption.UnitName;
                dto.Nutrient = GetNutrient(dto.ID);
                dto.NutrientID = dto.Nutrient.ID;
                dto.Fat = dto.Nutrient.Fat;
                dto.Protein = dto.Nutrient.Protein;
                dto.Carbs = dto.Nutrient.Carbs;
                dto.Sugar = dto.Nutrient.Sugar;
                dto.VitA = dto.Nutrient.VitA;
                dto.VitB = dto.Nutrient.VitB;
                dto.VitC = dto.Nutrient.VitC;
                dto.VitD = dto.Nutrient.VitD;
                dto.VitE = dto.Nutrient.VitE;
                dto.Na = dto.Nutrient.Na;
                dto.Potassium = dto.Nutrient.Potassium;
                dto.TagCategoryDetail = GetTagCategory(dto.ID);
                dto.TagName = dto.TagCategoryDetail.Name;
                dto.TagImage = dto.TagCategoryDetail.Image;

                return dto;
            }
        }
        public IQueryable<MealOption> GetTagMealsUnderMaximumCal(int mealTagID, int maxCal)
        {
            return db.MealTags.Where(mt => mt.MealTagCategoriesID == mealTagID).Select(mt => mt.MealOption).Where(m => m.Calories <= maxCal).Select(m => m);
        }
        public MealDetailDTO GetMealAndTags(int ID)
        {
            using (HealthHelperEntities db = new HealthHelperEntities())
            {
                MealOption mealOption = db.MealOptions.First(x => x.ID == ID);
                MealDetailDTO dto = new MealDetailDTO();
                dto.ID = mealOption.ID;
                dto.Name = mealOption.Name;
                dto.Calories = Convert.ToInt32(mealOption.Calories);
                dto.MealOptionImage = mealOption.Image;
                dto.UnitName = mealOption.UnitName;
                var list = db.MealTagCategories.ToList();
                List<TagCategoryDetailDTO> dtoList = new List<TagCategoryDetailDTO>();
                foreach (var item in list)
                {
                    TagCategoryDetailDTO Tagdto = new TagCategoryDetailDTO();
                    Tagdto.ID = item.ID;
                    Tagdto.Name = item.Name;
                    Tagdto.Image = item.Image;
                    dtoList.Add(Tagdto);
                }
                MealTagDAO mealTagDAO = new MealTagDAO();
                string tags = mealTagDAO.GetTagsListInString(dto.ID);
                dto.TagStringList = tags;
                dto.TagStringListArray = tags.Split('/');
                //dto.Tags = dtoList;
                return dto;


            }
        }
        public dynamic GetMealsByKeyword(string mealKeyword)
        {
            return db.MealOptions.Where(m => m.Name.Contains(mealKeyword)).Select(m => new {
                MealOptionID = m.ID,
                MealName = m.Name
            }).ToList();
        }
        public MealDetailDTO GetMealForAddWithTags(int ID)
        {
            using (HealthHelperEntities db = new HealthHelperEntities())
            {
                MealOption mealOption = db.MealOptions.First(x => x.ID == ID);
                MealDetailDTO dto = new MealDetailDTO();
                dto.ID = mealOption.ID;
                dto.Name = mealOption.Name;
                dto.Calories = Convert.ToInt32(mealOption.Calories);
                dto.MealOptionImage = mealOption.Image;
                dto.UnitName = mealOption.UnitName;
                dto.Nutrient = GetNutrient(dto.ID);
                dto.NutrientID = dto.Nutrient.ID;
                dto.Fat = dto.Nutrient.Fat;
                dto.Protein = dto.Nutrient.Protein;
                dto.Carbs = dto.Nutrient.Carbs;
                dto.Sugar = dto.Nutrient.Sugar;
                dto.VitA = dto.Nutrient.VitA;
                dto.VitB = dto.Nutrient.VitB;
                dto.VitC = dto.Nutrient.VitC;
                dto.VitD = dto.Nutrient.VitD;
                dto.VitE = dto.Nutrient.VitE;
                dto.Na = dto.Nutrient.Na;
                dto.Potassium = dto.Nutrient.Potassium;
                var list = db.MealTagCategories.ToList();
                foreach (var item in list)
                {
                    TagCategoryDetailDTO Tagdto = new TagCategoryDetailDTO();
                    Tagdto.ID = item.ID;
                    Tagdto.Name = item.Name;
                    dto.TagList.Add(Tagdto);
                }
                return dto;
            }
        }
        public int Add(MealOption meal)
        {
            try
            {
                db.MealOptions.Add(meal);
                db.SaveChanges();
                return meal.ID;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public MealOption GetMealByID(int id)
        {
            return db.MealOptions.FirstOrDefault(m => m.ID == id);
        }

        public void DeleteMeal(int ID)
        {
            MealOption mealOption = db.MealOptions.First(x => x.ID == ID);
            int nutrientID = (int)mealOption.NutrientID;

            NutrientDAO nutrientDAO = new NutrientDAO();
            nutrientDAO.DeleteNutrient(nutrientID);
            db.MealOptions.Remove(mealOption);
            db.SaveChanges();
        }
        public void HiddenMeal(int ID)
        {
            MealOption meal = db.MealOptions.First(x => x.ID == ID);
            meal.IsVisable = "Fales";
            db.SaveChanges();
        }
        public void DisplayMeal(int ID)
        {
            MealOption meal = db.MealOptions.First(x => x.ID == ID);
            meal.IsVisable = "True";
            db.SaveChanges();
        }

        public string UpdateMeal(MealDetailDTO entity)
        {
            MealOption meal = db.MealOptions.First(x => x.ID == entity.ID);
            meal.Name = entity.Name;
            meal.Calories = entity.Calories;
            meal.UnitName = entity.UnitName;
            string oldImagePass = meal.Image;
            if (entity.MealOptionImage != null)
            {
            meal.Image = entity.MealOptionImage;
            }
            db.SaveChanges();
            return oldImagePass;


        }
        public IQueryable<MealOption> GetLikedMeals(int memberID)
        {
            return db.LikedMeals.Where(lm => lm.MemberID == memberID).Select(lm => lm.MealOption);
        }

        public List<MealDetailDTO> GetOnlyMeals()
        {
            var list = db.MealOptions.ToList();

            List<MealDetailDTO> dtoList = new List<MealDetailDTO>();
            foreach (var item in list)
            {
                MealDetailDTO dto = new MealDetailDTO();
                dto.ID = item.ID;
                dto.Name = item.Name;
                dto.Calories = Convert.ToInt32(item.Calories);
                dto.MealOptionImage = item.Image;
                dto.UnitName = item.UnitName;
                dto.IsVisable = item.IsVisable;

                List<TagCategoryDetailDTO> TagList = new List<TagCategoryDetailDTO>();
                MealTagDAO mealTagDAO = new MealTagDAO();
                string tags = mealTagDAO.GetTagsListInString(dto.ID);
                dto.TagStringList = tags;
                dtoList.Add(dto);
            }
            return dtoList;
        }

        public List<MealDetailDTO> GetMealsByTag(int TagID)
        {
            List<MealDetailDTO> mealDetailList = new List<MealDetailDTO>();
            var list = db.MealTags.Where(x => x.MealTagCategoriesID == TagID).ToList();
            var TagList = db.MealTagCategories.Where(x => x.ID == TagID).First();
            foreach(var item in list)
            {
                var mealList = db.MealOptions.Where(x => x.ID == item.MealOptionID).ToList();
                foreach (var meal in mealList)
                {
                    MealDetailDTO mealDto = new MealDetailDTO();
                    mealDto.Name = meal.Name;
                    mealDto.Calories = (int)meal.Calories;
                    mealDto.MealOptionImage = meal.Image;
                    mealDto.TagID = TagID;
                    mealDto.ID = meal.ID;
                    mealDto.TagName = TagList.Name;
                    mealDetailList.Add(mealDto);
                }
            }
            return mealDetailList;
        }


        public void AddTag(int mealID, int categoryID)
        {
            try
            {
                MealTag tag = new MealTag();
                tag.MealOptionID = mealID;
                tag.MealTagCategoriesID = categoryID;
                db.MealTags.Add(tag);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        
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
                DateTime st = (DateTime)pg.StartDate;
              DateTime end = (DateTime)pg.EndDate;
               

            winnerDietLogs.AddRange(db.DietLogs.Where(dl => dl.MemberID == pg.MemberID).ToList().Where(dl => DateTime.ParseExact(dl.Date, CDictionary.MMddyyyy, CultureInfo.InvariantCulture) >= st && DateTime.ParseExact(dl.Date, CDictionary.MMddyyyy, CultureInfo.InvariantCulture) <= end).ToList());


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
            var target = db.LikedMeals.FirstOrDefault(lm => lm.MemberID == memberId && lm.MealOptionID == mealId);
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



        public bool checkMealName(string userInput)
        {
            var meal = db.MealOptions.Any(x => x.Name == userInput);
            if(meal)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool IsLikedMeal(int memberId, int mealId)
        {
            using (HealthHelperEntities db = new HealthHelperEntities())
            {
                return db.LikedMeals.Any(lm => lm.MemberID == memberId && lm.MealOptionID == mealId);
            }
        }
    }
}
