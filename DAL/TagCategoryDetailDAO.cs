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
        public void AddMealTags(int mealID,string[] Tags)
        {
            List<MealTagDTO> Taglist = new List<MealTagDTO>();
            MealTagDTO mealTagDTO = new MealTagDTO();
            MealTag mealTag = new MealTag();
            for(int i = 0; i < Tags.Length ; i++)
            {
                mealTag.MealOptionID = mealID;
                mealTag.MealTagCategoriesID = Convert.ToInt32(Tags[i]);
                db.MealTags.Add(mealTag);
                db.SaveChanges();
            }
        }
        public void UpdataMealTags(int mealID,string[] Tags)
        {
            List<MealTagDTO> tagList = new List<MealTagDTO>();
            MealTagDTO mealTagDTO = new MealTagDTO();
            MealTag mealTag = new MealTag();
            for (int i = 0; i < Tags.Length; i++)
            {
                mealTag.MealOptionID = mealID;
                mealTag.MealTagCategoriesID = Convert.ToInt32(Tags[i]);
                db.MealTags.Add(mealTag);
                db.SaveChanges();
            }
        }

        public void DeleteMealInTag(int mealID, int TagID)
        {
            MealTag mealTag = db.MealTags.Where(x => x.MealTagCategoriesID == TagID && x.MealOptionID==mealID).First();
            db.MealTags.Remove(mealTag);
            db.SaveChanges();
            
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
        public List<TagCategoryDetailDTO> GetTagsForAddMeal()
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
        public bool HasTag(int mealID, int categoryID)
        {
            MealTag tag = db.MealTags.FirstOrDefault(x => x.MealOptionID == mealID && x.MealTagCategoriesID == categoryID);
            if (tag != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public void RemoveTag(int mealID, int categoryID)
        {
            try
            {
                MealTag tag = db.MealTags.First(x => x.MealOptionID == mealID && x.MealTagCategoriesID == categoryID);
                db.MealTags.Remove(tag);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void AddMealToTag(int mealID,int categoryID)
        {
            try 
            {
                MealTagDTO dto = new MealTagDTO();
                MealTag mealTag = new MealTag();
                mealTag.MealOptionID = mealID;
                mealTag.MealTagCategoriesID = categoryID;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
        public List<MealTagDTO> GetMealAllTags(int mealID)
        {
            var tagList = db.MealTags.Where(x => x.MealOptionID == mealID).ToList();
           
            List<MealTagDTO> dtoList = new List<MealTagDTO>();
            foreach(var item in tagList)
            {
                MealTagDTO dto = new MealTagDTO();
                dto.MealOptionID = item.MealOptionID;
                dto.MealTagCategoriesID = item.MealTagCategoriesID;
                dtoList.Add(dto);
            }
            return dtoList;
        }
        public void UpdateMealTags(int mealID, string[] Tags)
        {
            List<MealTagDTO> mealTagDTOList = new List<MealTagDTO>();
              mealTagDTOList = GetMealAllTags(mealID); //取得這個餐點所有的標籤 in List
            
            foreach(var item in mealTagDTOList)
            {
                bool checkTagInTags = false;
                for(int num = 0; num < Tags.Length; num++)
                {
                    if(item.MealTagCategoriesID == Convert.ToInt32(Tags[num]))
                    {
                        checkTagInTags = true;
                    }
                }
                if (!checkTagInTags) //如果DB有資料但是沒選取則刪除
                {
                    RemoveTag(mealID, item.MealTagCategoriesID);
                }
                for(int i = 0; i < Tags.Length; i++)
                {
                    if (!HasTag(mealID, Convert.ToInt32(Tags[i]))) //沒有這個標籤
                    {
                        AddTag(mealID, Convert.ToInt32(Tags[i])); //新增標籤
                    }
                }

                //for (int i = 0; i < Tags.Length; i++)  //跑所有有勾選的標籤
                //{
                //    if (!HasTag(mealID, Convert.ToInt32(Tags[i]))) //沒有這個標籤
                //    {
                //        AddTag(mealID, Convert.ToInt32(Tags[i])); //新增標籤
                //    }
                //    else
                //    {
                //        if (HasTag(mealID, Convert.ToInt32(Tags[i]))) //有這個標籤
                //        {
                //            RemoveTag(mealID, Convert.ToInt32(Tags[i]));
                //        }
                //    }
                //}
            }

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
