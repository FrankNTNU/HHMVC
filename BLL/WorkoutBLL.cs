using DAL;

using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace BLL
{
    public class WorkoutBLL
    {
        WorkoutDAO dao = new WorkoutDAO();
        ActivityLevelDAO activityLevelDAO = new ActivityLevelDAO();
        WorkoutCategoryDAO workoutCatDAO = new WorkoutCategoryDAO();

        public List<WorkoutItemDTO> GetWorkouts()
        {
            List<WorkoutItemDTO> itemList = new List<WorkoutItemDTO>();
            //dtoList = dao.GetWorkouts();
            itemList = dao.GetWorkouts();
            return itemList;
        }
        public List<WorkoutItemDTO> GetWorkouts(int ID)
        {
            List<WorkoutItemDTO> itemList = new List<WorkoutItemDTO>();
            //dtoList = dao.GetWorkouts();
            itemList = dao.GetWorkouts(ID);
            return itemList;
        }

        public bool Add(WorkoutItemDTO entity)
        {
            Workout workout = new Workout();
            workout.Name = entity.Name;
            workout.Calories = entity.Calories;
            workout.ActivityLevelID = entity.ActivityLevelID;
            //workout.ActivityLevel.Description = entity.ActivityLevel;
            //workout.WorkoutCategory.Name = entity.CategoryName;
            workout.WorkoutCategoryID = (int)entity.CategoryID;
            return dao.Add(workout);
        }

        public WorkoutDTO GetWorkouts(string text)
        {
            WorkoutDTO dto = new WorkoutDTO();
            dto.WorkoutItems = dao.GetWorkouts(text);
            dto.ActivityLevels = activityLevelDAO.GetLevels();
            dto.Categories = workoutCatDAO.GetCategories();
            return dto;
        }

        public void Update(WorkoutItemDTO entity)
        {
            Workout workout = new Workout();
            workout.ID = entity.ID;
            workout.Name = entity.Name;
            workout.Calories = entity.Calories;
            workout.ActivityLevelID = entity.ActivityLevelID;
            workout.WorkoutCategoryID = (int)entity.CategoryID;
            dao.Update(workout);
        }

        public WorkoutItemDTO GetWorkoutWithID(int ID)
        {
            return dao.GetWorkoutWithID(ID);
        }


        public bool Delete(int ID)
        {
            return dao.Delete(ID);
        }

        public bool IsWorkoutExist(string name)
        {
            return dao.IsWorkoutExist(name);
        }

        public static IEnumerable<SelectListItem> GetWorkoutNamesForDropDown()
        {
            return WorkoutDAO.GetWorkoutNamesForDropDown();
        }

        //public int GetCatIDByWorkID(int ID)
        //{
        //    return dao.GetCatIDByWorkID(ID);
        //}
    }
}
