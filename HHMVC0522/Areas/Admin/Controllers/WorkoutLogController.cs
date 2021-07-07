using BLL;
using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace UI.Areas.Admin.Controllers
{
    public class WorkoutLogController : Controller
    {
        // GET: Admin/WorkoutLog
        WorkoutLogBLL workoutLogBLL = new WorkoutLogBLL();
        WorkoutBLL workoutBLL = new WorkoutBLL();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult List()
        {
            List<WorkoutLogDTO> logList = new List<WorkoutLogDTO>();
            WorkoutDTO dto = new WorkoutDTO();
            logList = workoutLogBLL.GetWorkoutLogs();
            return View(logList);
        }
        public ActionResult Delete(int ID)
        {
            WorkoutLogDTO dto = new WorkoutLogDTO();
            workoutLogBLL.Delete(ID);
            workoutLogBLL = new WorkoutLogBLL();
            return RedirectToAction("List");
        }
        public ActionResult Edit(int ID)
        {
            WorkoutLogDTO dto = new WorkoutLogDTO();
            
            dto = workoutLogBLL.GetWorkoutLogWithID(ID);
            dto.WorkoutNames = WorkoutBLL.GetWorkoutNamesForDropDown();
            return View(dto);
        }
        [HttpPost]
        public ActionResult Edit(WorkoutLogDTO model)
        {
            workoutLogBLL.UpDate(model);
            workoutLogBLL = new WorkoutLogBLL();
            return RedirectToAction("List");
        }

        public ActionResult Create()
        {

            return View();
        }
        [HttpPost]
        public ActionResult Create(WorkoutLogDTO model)
        {
            workoutLogBLL.Add(model);           
            return RedirectToAction("List");
        }
    }
}