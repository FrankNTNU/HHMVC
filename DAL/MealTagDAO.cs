using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO;
using DAL;

namespace DAL
{
    public class MealTagDAO:HHContext
    {
        public TagCategoryDetailDTO GetTags(int ID)
        {
            //MealOption meal = db.MealOptions.FirstOrDefault(x => x.ID == ID);
            //TagCategoryDetailDTO dto = new TagCategoryDetailDTO();
            //int mealID = meal.ID;
            //MealTagCategory tag = db.MealTagCategories.First(x => x.ID == mealID);
            //if (tag != null)
            //{
            //    dto.Name = tag.Name;
            //    dto.Image = tag.Image;
            //}
            //    return dto;


            var tagList = db.MealTags.Where(x => x.MealOptionID == ID).ToList();
                TagCategoryDetailDTO tagDTO = new TagCategoryDetailDTO();
            foreach (var tag in tagList)
            {
         
                tagDTO.ID = tag.ID;
                tagDTO.Name = tag.MealTagCategory.Name;
                tagDTO.Image = tag.MealTagCategory.Image;
                
            }
            return tagDTO;
        }
        public List<TagCategoryDetailDTO> GetTagsList(int ID)
        {
            //MealOption meal = db.MealOptions.FirstOrDefault(x => x.ID == ID);
            //TagCategoryDetailDTO dto = new TagCategoryDetailDTO();
            //int mealID = meal.ID;
            //MealTagCategory tag = db.MealTagCategories.First(x => x.ID == mealID);
            //if (tag != null)
            //{
            //    dto.Name = tag.Name;
            //    dto.Image = tag.Image;
            //}
            //    return dto;


            var tagList = db.MealTags.Where(x => x.MealOptionID == ID).ToList();
            TagCategoryDetailDTO tagDTO = new TagCategoryDetailDTO();
            List<TagCategoryDetailDTO> list = new List<TagCategoryDetailDTO>();
            foreach (var tag in tagList)
            {
                tagDTO.ID = tag.ID;
                tagDTO.Name = tag.MealTagCategory.Name;
                tagDTO.Image = tag.MealTagCategory.Image;
                list.Add(tagDTO);
            }
            return list;
        }
    }
}
