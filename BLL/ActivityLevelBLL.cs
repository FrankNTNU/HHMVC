using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace BLL
{
    public class ActivityLevelBLL
    {
        public static IEnumerable<SelectListItem> GetActivityLevelsForDropDown()
        {
            return ActivityLevelDAO.GetActivityLevelsForDropDown();
        }
    }
}
