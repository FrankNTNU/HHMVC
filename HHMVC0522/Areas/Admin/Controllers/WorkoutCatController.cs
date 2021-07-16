using BLL;
using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace UI.Areas.Admin.Controllers
{
    public class WorkoutCatController : Controller
    {
        WorkoutCategoryBLL categoryBll ;
        // GET: Admin/Workout
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult List()
        {
            List<WorkoutCategoryDTO> categoryList = new List<WorkoutCategoryDTO>();
            categoryBll = new WorkoutCategoryBLL();
            categoryList = categoryBll.GetCategories();
            return View(categoryList);
        }
        public ActionResult Create()
        {

            return View();
        }
        [HttpPost]
        public ActionResult Create(WorkoutCategoryDTO model)
        {
            categoryBll = new WorkoutCategoryBLL();
            if (string.IsNullOrEmpty(model.Name))
                return RedirectToAction("List");

            if (categoryBll.IsCategoryExist(model.Name))
            {
                ViewBag.ProcessState = General.Messages.SameName;
                return View(model);
            }
            else
            {
            categoryBll.Add(model);
            categoryBll = new WorkoutCategoryBLL();
            return RedirectToAction("List");
            }

        }
        public ActionResult Edit(int ID)
        {
            categoryBll = new WorkoutCategoryBLL();
            WorkoutCategoryDTO dto = new WorkoutCategoryDTO();
            dto = categoryBll.GetWorkoutCayWithID(ID);
            return View(dto);
        }
        [HttpPost]
        public ActionResult Edit(WorkoutCategoryDTO model)
        {
            categoryBll = new WorkoutCategoryBLL();
            categoryBll.Update(model);
            categoryBll = new WorkoutCategoryBLL();
            return RedirectToAction("List");
        }
        public ActionResult Delete(int ID)
        {
            categoryBll = new WorkoutCategoryBLL();
            WorkoutCategoryDTO dto = new WorkoutCategoryDTO();
            categoryBll.Delete(ID);
            categoryBll = new WorkoutCategoryBLL();
            return RedirectToAction("List");
        }
        public JsonResult IsCategoryExist(string text)
        {
            string exist = "";
            if(categoryBll.IsCategoryExist(text))
            {
                exist = "true";
            }
            else
            {
                exist = "false";
            }
            return Json(exist, JsonRequestBehavior.AllowGet);
        }
    }
}