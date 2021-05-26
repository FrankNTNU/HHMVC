using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace UI.Controllers
{
    public class WorkoutController : Controller
    {
        // GET: Workout
        public ActionResult WorkoutLog()
        {
            return View();
        }
    }
}