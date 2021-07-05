
using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace BLL
{
    public class TimeOfDayBLL
    {
        TimeOfDayDAO dao = new TimeOfDayDAO();
        public IQueryable<TimesOfDay> GetTimeOfDays()
        {
            return dao.GetTimeOfDays();
        }

        public  IEnumerable<SelectListItem> GetTimesOfDayForDropDown()
        {
            return dao.GetTimesOfDayForDropDown();
        }
    }
}
