using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace DAL
{
    public class TagCategoryDetailDAO:HHContext
    {
        public List<TagCategoryDetailDTO> GetTags()
        {
            var list = db.MealTagCategories.ToList();
            List<TagCategoryDetailDTO> dtoList = new List<TagCategoryDetailDTO>();
            foreach (var item in list)
            {
                TagCategoryDetailDTO dto = new TagCategoryDetailDTO();
                dto.ID = item.ID;
                dto.Name = item.Name;
                dto.Image = item.Image;
                dtoList.Add(dto);
            }
            return dtoList;
        }
        public TagCategoryDetailDTO GetTags(int ID)
        {
            TagCategoryDetailDTO dto = new TagCategoryDetailDTO();
            MealTagCategory mealTagCategory = db.MealTagCategories.First(x => x.ID == ID);

            dto.Name = mealTagCategory.Name;
            dto.Image = mealTagCategory.Image;

            return dto;
        }
        //public List<TagCategoryDetailDTO> GetTagList(int ID)
        //{
        //    List<TagCategoryDetailDTO> dtoloist = new List<TagCategoryDetailDTO>();
        //    TagCategoryDetailDTO dto = new TagCategoryDetailDTO();
        //    MealTagCategory mealTagCategory = db.MealTagCategories(x => x.ID == ID);

        //    dto.Name  = mealTagCategory.Name;
        //    dto.Image = mealTagCategory.Image;

        //    return dto;
        //}
        public void AddTag(MealTagCategory entity)
        {
            db.MealTagCategories.Add(entity);
            db.SaveChanges();
        }
        public string Delete(int ID)
        {
            MealTagCategory mealTagCategory = db.MealTagCategories.First(x => x.ID == ID);
            string deleteImagePass = mealTagCategory.Image;
            db.MealTagCategories.Remove(mealTagCategory);
            List<MealTag> mealTagList = db.MealTags.Where(x => x.MealTagCategoriesID == ID).ToList();
            db.MealTags.RemoveRange(mealTagList);

            db.SaveChanges();
            return deleteImagePass;
        }
        public string Update(TagCategoryDetailDTO entity)
        {
            MealTagCategory mealTagCategory = db.MealTagCategories.First(x => x.ID == entity.ID);

            mealTagCategory.Name = entity.Name;
            string oldImagePass = mealTagCategory.Image;
            if (entity.UpLoadImage != null)
            {
                mealTagCategory.Image = entity.Image;
            }
            db.SaveChanges();
            return oldImagePass;
        }
    }
}
