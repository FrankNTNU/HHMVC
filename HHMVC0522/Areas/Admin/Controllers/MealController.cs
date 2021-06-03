using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BLL;
using DTO;

namespace UI.Areas.Admin.Controllers
{
    public class MealController : Controller
    {
        // GET: Admin/Meal
        public ActionResult List()
        {
            MealBLL bll = new MealBLL();
            List<MealDetailDTO> dto = new List<MealDetailDTO>();
            dto = bll.GetMeals();
            
            return View(dto);
        }
        public ActionResult Create()
        {
            return View();
        }
        public ActionResult Update()
        {
            return View();
        }
    }
}