using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace DAL
{
    public class TimeOfDayDAO:HHContext
    {
        public IQueryable<TimesOfDay> GetTimeOfDays()
        {
            return db.TimesOfDays.OrderBy(tod=>tod.ID).Select(tod => tod);
        }


        public  IEnumerable<SelectListItem> GetTimesOfDayForDropDown()
        {
            IEnumerable<SelectListItem> TimesOfDay = db.TimesOfDays
                .Select(x => new SelectListItem()
                {
                    Text = x.Name,
                    Value = x.ID.ToString()
                }).ToList().OrderBy(t=>t.Value);
            return TimesOfDay;
        }
    }
}
