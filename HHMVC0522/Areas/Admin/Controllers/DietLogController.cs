using BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace UI.Areas.Admin.Controllers
{
    public class DietLogController : Controller
    {
        // GET: Admin/DietLog
        public ActionResult List()
        {
            DietLogBLL bll = new DietLogBLL();
            return View(bll.GetAllDietLog());
        }
        public void Delete(int ID)
        {
            DietLogBLL bll = new DietLogBLL();
            bll.DeleteDietLog(ID);
            
        }
    }
}