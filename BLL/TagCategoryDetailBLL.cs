using DAL;
using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class TagCategoryDetailBLL
    {
        public List<TagCategoryDetailDTO> GetTags()
        {
            TagCategoryDetailDAO dao = new TagCategoryDetailDAO();
            return dao.GetTags();
        }
        public TagCategoryDetailDTO GetTags(int ID)
        {
            TagCategoryDetailDAO dao = new TagCategoryDetailDAO();
            return dao.GetTags(ID);
        }
        public void AddTags(TagCategoryDetailDTO dto)
        {
            MealTagCategory tag = new MealTagCategory();
            TagCategoryDetailDAO dao = new TagCategoryDetailDAO();
            tag.Name = dto.Name;
            tag.Image = dto.Image;
            dao.AddTag(tag);
        }
        public string Delete(int ID)
        {
           TagCategoryDetailDAO dao = new TagCategoryDetailDAO();
           return dao.Delete(ID);
        }
        public void DeleteMealInTag(int mealID, int TagID)
        {
            TagCategoryDetailDAO dao = new TagCategoryDetailDAO();
            dao.DeleteMealInTag(mealID, TagID);
        }
        public string Update(TagCategoryDetailDTO dto)
        {
            TagCategoryDetailDAO dao = new TagCategoryDetailDAO();
            return dao.Update(dto);
        }
        public List<TagCategoryDetailDTO> GetTagForAddMeal()
        {
            TagCategoryDetailDAO dao = new TagCategoryDetailDAO();
            List<TagCategoryDetailDTO> list = new List<TagCategoryDetailDTO>();
            list = dao.GetTagsForAddMeal();
            return list;
        }
        public List<MealDetailDTO> GetMealsByTag(int ID)
        {
            MealDAO dao = new MealDAO();
            return dao.GetMealsByTag(ID);
        }
        public void AddMealTags(int mealID,string[] Tags)
        {
            TagCategoryDetailDAO dao = new TagCategoryDetailDAO();
            dao.AddMealTags(mealID, Tags);
        }
        public void UpdateMealTags(int mealID,string[] Tags)
        {
            TagCategoryDetailDAO dao = new TagCategoryDetailDAO();
            dao.UpdateMealTags(mealID, Tags);

        }
        //public void AddMealTags(int mealID, string[] Tags)
        //{
        //    List<MealTagDTO> Taglist = new List<MealTagDTO>();
        //    MealTagDTO mealTagDTO = new MealTagDTO();
        //    MealTag mealTag = new MealTag();
        //    for (int i = 0; i < Tags.Length; i++)
        //    {
        //        mealTag.MealOptionID = mealID;
        //        mealTag.MealTagCategoriesID = Convert.ToInt32(Tags[i]);
        //        db.MealTags.Add(mealTag);
        //        db.SaveChanges();
        //    }
        //}
    }
}
