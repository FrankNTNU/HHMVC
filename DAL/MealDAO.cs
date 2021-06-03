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
                dto.VitA = dto.Nutrient.VitA;
                dto.VitB = dto.Nutrient.VitB;
                dto.VitC = dto.Nutrient.VitC;
                dto.VitD = dto.Nutrient.VitD;
                dto.VitE = dto.Nutrient.VitE;
                dto.Na = dto.Nutrient.Na;
                dto.K = dto.Nutrient.K;
                dto.TagCategoryDetail = GetTagCategory(dto.ID);
                dto.TagName = dto.TagCategoryDetail.Name;
                dto.TagImage = dto.TagCategoryDetail.Image;


                //List<TagCategoryDetailDTO> tagDetailList = new List<TagCategoryDetailDTO>();
                //TagCategoryDetailDTO tdto = new TagCategoryDetailDTO();
                //var tagList = db.MealTags.Where(x => x.MealOptionID == item.ID).ToList();
                //foreach (var tag in tagList)
                //{
                //    dto.TagName = tag.MealTagCategory.Name;
                //    dto.TagCategoryDetail.Image = tag.MealTagCategory.Image;
                //}
                dtoList.Add(dto);
            }
            return dtoList;
        }
    }
}
