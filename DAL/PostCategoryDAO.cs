using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace DAL
{
    public class PostCategoryDAO : HHContext
    {
        public static IEnumerable<SelectListItem> GetPostCategoriesForDropDown()
        {
            IEnumerable<SelectListItem> postCategories = db.PostCategories
                .Select(x => new SelectListItem()
                {
                    Text = x.Name,
                    Value = x.ID.ToString()
                }).ToList();
            return postCategories;
        }
    }
}
