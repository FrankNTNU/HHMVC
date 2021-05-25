using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace DAL
{
    public class CommentCategoryDAO : HHContext
    {
        public static IEnumerable<SelectListItem> GetCategoryiesForDrop()
        {
            IEnumerable<SelectListItem> categories = db.CommentCategories
                .Select(x => new SelectListItem()
                {
                    Text = x.Name,
                    Value = x.ID.ToString()
                }).ToList();
            return categories;
        }
    }
}
