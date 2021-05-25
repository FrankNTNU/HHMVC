using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace DAL
{
    public class ActivityLevelDAO : HHContext
    {
        public static IEnumerable<SelectListItem> GetActivityLevelsForDropDown()
        {
            IEnumerable<SelectListItem> activityLevels = db.ActivityLevels
                .Select(x => new SelectListItem()
                {
                    Text = x.Description,
                    Value = x.ID.ToString()
                }).ToList();
            return activityLevels;
        }
    }
}
