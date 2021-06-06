using BLL;
using DAL;
using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace UI.Areas.Admin.Controllers
{
    public class NutrientController : Controller
    {
        // GET: Admin/Nutrient
        public ActionResult List()
        {
            MealBLL bll = new MealBLL();
            List<MealDetailDTO> dto = new List<MealDetailDTO>();
            dto = bll.GetMeals();

            return View(dto);
        }
        public ActionResult UpdateNutrient(int ID)
        {
            //NutrientBLL bll = new NutrientBLL();
            //bll.GetNutrientUseNurtientID(ID);
            //return View();
            MealBLL bll = new MealBLL();
            MealDetailDTO dto = new MealDetailDTO();
            dto = bll.GetMeals(ID);
            return View(dto);
        }
        [HttpPost]
        public ActionResult UpdateNutrient(NutrientDTO dto)
        {
            NutrientBLL bll = new NutrientBLL();
            bll.UpdateNutrient(dto);
            return RedirectToAction("List");
        }
    }
}