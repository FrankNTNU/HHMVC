using BLL;
using DTO;
using System;
using System.Web.Mvc;
using UI.ViewModels;

namespace UI.Controllers
{
    public class HHDietHistoryController : Controller
    {
        DietLogBLL dlBLL = new DietLogBLL();

        // GET: HHDeitHistory
        public ActionResult DietLogsHistory()
        {

            int memberID = 17; //todo  test
            if (Session["ID"] != null)
            {
                memberID = (int)Session["ID"];
            }
            DietLogsHistoryViewModel model = new DietLogsHistoryViewModel(memberID, true,true);

            return View(model); 
        }

        //public ActionResult Delete(int id)
        //{
        //    dlBLL.DeleteDietLog(id);
        //    return RedirectToAction("List");
        //}
    }
}