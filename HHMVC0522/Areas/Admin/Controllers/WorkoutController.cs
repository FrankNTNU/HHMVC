using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BLL;
using DTO;
using DAL;

namespace UI.Areas.Admin.Controllers
{
    public class WorkoutController : Controller
    {
        WorkoutBLL workoutBLL = new WorkoutBLL();
        WorkoutCategoryBLL categoryBll = new WorkoutCategoryBLL();
        // GET: Admin/Workout
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult List(int ID=6)
        {
            using (HealthHelperEntities db = new HealthHelperEntities())
            {
                ViewBag.CatName = db.WorkoutCategories.Where(m => m.ID == ID).FirstOrDefault().Name;
                ViewBag.CatID = db.WorkoutCategories.Where(m => m.ID == ID).FirstOrDefault().ID;
            }
            workoutBLL = new WorkoutBLL();
            WorkoutDTO dto = new WorkoutDTO();
            //WorkoutItemDTO itemDTO = new WorkoutItemDTO();
            List<WorkoutItemDTO> itemList = new List<WorkoutItemDTO>();
            dto.WorkoutItems = workoutBLL.GetWorkouts(ID);
            dto.Categories = categoryBll.GetCategories();
            //dto.WorkoutItems = workoutBLL.GetWorkouts(ID);
            return View(dto);

        }
        public ActionResult Create(int id)
        {
            ViewBag.CatID= HHContext.db.WorkoutCategories.Where(m => m.ID == id).FirstOrDefault().ID;

            WorkoutItemDTO dtO = new WorkoutItemDTO();
            dtO.ActivityLevelNames= ActivityLevelBLL.GetActivityLevelsForDropDown();
            dtO.WorkoutCategories = WorkoutCategoryBLL.GetWorkoutCategoriesForDropDown();
            
            return View(dtO);           
        }
        [HttpPost]
        public ActionResult Create(WorkoutItemDTO model)
        {
            if (string.IsNullOrEmpty(model.Name))
                return RedirectToAction("List");
            if(workoutBLL.IsWorkoutExist(model.Name))
            {
                model.ActivityLevelNames = ActivityLevelBLL.GetActivityLevelsForDropDown();
                model.WorkoutCategories = WorkoutCategoryBLL.GetWorkoutCategoriesForDropDown();
                ViewBag.ProcessState = General.Messages.SameName;               
                return View(model);
            }
            else
            {
            workoutBLL.Add(model);
            int a = (int)model.CategoryID;
            workoutBLL = new WorkoutBLL();
            return RedirectToAction("List/" + a.ToString());
            }

        }
        public ActionResult Delete(int ID)
        {
            //WorkoutItemDTO dto = new WorkoutItemDTO();
            int catID = workoutBLL.GetCatIDByWorkID(ID);
            workoutBLL.Delete(ID);
            //int a = (int)dto.CategoryID;
            //workoutBLL = new WorkoutBLL();
            return RedirectToAction("List", new { ID = catID });
        }
        public ActionResult Edit(int ID)
        {
            WorkoutItemDTO dto = new WorkoutItemDTO();
            dto= workoutBLL.GetWorkoutWithID(ID);
            dto.ActivityLevelNames = ActivityLevelBLL.GetActivityLevelsForDropDown();
            dto.WorkoutCategories = WorkoutCategoryBLL.GetWorkoutCategoriesForDropDown();

            return View(dto);
        }
        [HttpPost]
        public ActionResult Edit(WorkoutItemDTO model)
        {
            workoutBLL.Update(model);
            int a = (int)model.CategoryID;
            workoutBLL = new WorkoutBLL();
            return RedirectToAction("List",new {ID=a });
        }
    }
}