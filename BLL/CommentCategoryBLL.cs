using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace BLL
{
    public static class CommentCategoryBLL
    {
        public static IEnumerable<SelectListItem> GetCategoriesForDropDown()
        {
            return CommentCategoryDAO.GetCategoryiesForDrop();
        }
    }
}
