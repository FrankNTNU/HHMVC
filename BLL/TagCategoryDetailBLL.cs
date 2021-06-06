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
        public string Update(TagCategoryDetailDTO dto)
        {
            TagCategoryDetailDAO dao = new TagCategoryDetailDAO();
            return dao.Update(dto);
        }
    }
}
