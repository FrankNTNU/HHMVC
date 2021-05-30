using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace UI.Areas.Admin.Controllers
{
    public class MealController : Controller
    {
        // GET: Admin/Meal
        public ActionResult Index()
        {
            return View();
        }
    }
}