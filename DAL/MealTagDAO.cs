using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO;
using DAL;

namespace DAL
{
    public class MealTagDAO: HHContext
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

            string tagStringList = "";
            var tagList = db.MealTags.Where(x => x.MealOptionID == ID).ToList();
            TagCategoryDetailDTO tagDTO = new TagCategoryDetailDTO();
            List<TagCategoryDetailDTO> list = new List<TagCategoryDetailDTO>();
            foreach (var tag in tagList)
            {
                tagDTO.ID = tag.ID;
                tagDTO.Name = tag.MealTagCategory.Name;
                tagDTO.Image = tag.MealTagCategory.Image;
                tagStringList += tag.MealTagCategory.Name;
                tagStringList += "/";
                list.Add(tagDTO);
            }
            return list;
        }
        public IQueryable<MealTagCategory> GetAllTags()
        {
            var q = from mtc in db.MealTagCategories
                    select mtc;
            return q;
        }


        public string GetTagsListInString(int ID)
        {

            string tagStringList = "";
            var tagList = db.MealTags.Where(x => x.MealOptionID == ID).ToList();
            TagCategoryDetailDTO tagDTO = new TagCategoryDetailDTO();
            foreach (var tag in tagList)
            {
                tagStringList += tag.MealTagCategory.Name;
                tagStringList += "/";
            }
            if (tagStringList != "")
            {
                tagStringList = tagStringList.Substring(0, tagStringList.Length - 1);
            }
            return tagStringList;
        }
        public MealTagCategory GetTagByID(int id)
        {
            return db.MealTagCategories.FirstOrDefault(mtc => mtc.ID == id);
        }
    }
}
