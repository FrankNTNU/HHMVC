using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace DAL
{
    public class StatusDAO : HHContext
    {
        public static IEnumerable<SelectListItem> GetStatusesForDropDown()
        {
            IEnumerable<SelectListItem> statuses = db.Status
                .Select(x => new SelectListItem()
                {
                    Text = x.Name,
                    Value = x.ID.ToString()
                }).ToList();
            return statuses;
        }
        public string GetStatus(int ID)
        {
            var status = db.Status.First(x => x.ID == ID);
            return status.Name;
        }
    }
}
