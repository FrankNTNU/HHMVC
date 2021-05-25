using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace BLL
{
    public class StatusBLL
    {
        public static IEnumerable<SelectListItem> GetStatusesForDropDown()
        {
            return StatusDAO.GetStatusesForDropDown();
        }
    }
}
