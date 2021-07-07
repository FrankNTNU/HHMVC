using DAL;
using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace DAL
{
    public class WorkoutDAO : HHContext
    {

        public List<WorkoutItemDTO> GetWorkouts()
        {
            List<Workout> workouts = db.Workouts.ToList();
            List<WorkoutItemDTO> workoutDTOList = new List<WorkoutItemDTO>();


            foreach (var item in workouts)
            {
                WorkoutItemDTO dto = new WorkoutItemDTO();
                dto.ID = item.ID;
                dto.Name = item.Name;
                dto.Calories = item.Calories;
                dto.CategoryID = item.WorkoutCategoryID;
                dto.CategoryName = item.WorkoutCategory.Name;
                dto.ActivityLevelID = item.ActivityLevelID;
                dto.ActivityLevel = item.ActivityLevel.Description;
                workoutDTOList.Add(dto);
            }
            return workoutDTOList;
        }


        public bool Add(Workout entity)
        {
            try
            {
                db.Workouts.Add(entity);
                db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public List<WorkoutItemDTO> GetWorkouts(string text)
        {
            List<Workout> workouts = db.Workouts.Where(x => x.Name.Contains(text) || x.WorkoutCategory.Name.Contains(text)) .ToList();
            List<WorkoutItemDTO> workoutDTOList = new List<WorkoutItemDTO>();
            foreach (var item in workouts)
            {
                WorkoutItemDTO dto = new WorkoutItemDTO();
                dto.ID = item.ID;
                dto.Name = item.Name;
                dto.Calories = item.Calories;
                dto.CategoryID = item.WorkoutCategoryID;
                dto.CategoryName = item.WorkoutCategory.Name;
                dto.ActivityLevelID = item.ActivityLevelID;
                dto.ActivityLevel = item.ActivityLevel.Description;
                workoutDTOList.Add(dto);
            }
            return workoutDTOList;
        }

        //public int GetCatIDByWorkID(int iD)
        //{
            
        //}

        //public WorkoutCategoryDTO GetWorkoutItemWithID(object iD)
        //{
        //                List<Workout> list = db.Workouts.Where(x => x.WorkoutCategoryID == ID).ToList();
        //    List<WorkoutItemDTO> dtoList = new List<WorkoutItemDTO>();
        //    foreach (Workout item in list)
        //    {
        //        WorkoutItemDTO dto = new WorkoutItemDTO();
        //        dto.ID = item.ID;
        //        dto.Name = item.Name;
        //        dto.Calories = item.Calories;
        //        dtoList.Add(dto);
        //    }
        //    return dtoList;
        //}
        public WorkoutItemDTO GetWorkoutWithID(int ID)
        {
            WorkoutItemDTO dto = new WorkoutItemDTO();
            Workout workout = db.Workouts.FirstOrDefault(x => x.ID == ID);
            if (workout != null)
            {
                dto.ID = workout.ID;
                dto.Name = workout.Name;
                dto.CategoryID = workout.WorkoutCategoryID;
                dto.CategoryName = workout.WorkoutCategory.Name;
                dto.ActivityLevelID = workout.ActivityLevelID;
                dto.ActivityLevel = workout.ActivityLevel.Description;
                dto.Calories = workout.Calories;
            }
            return dto;
        }
        public List<WorkoutItemDTO> GetWorkouts(int ID)
        {
            List<Workout> list = db.Workouts.Where(x => x.WorkoutCategoryID == ID).ToList();
            List<WorkoutItemDTO> dtoList = new List<WorkoutItemDTO>();

            foreach (var item in list)
            {
                WorkoutItemDTO dto = new WorkoutItemDTO();
                dto.CategoryID = item.WorkoutCategoryID;
                dto.CategoryName = item.WorkoutCategory.Name;
                dto.ActivityLevel = item.ActivityLevel.Description;
                dto.ID = item.ID;
                dto.Name = item.Name;
                dto.Calories = item.Calories;
                dtoList.Add(dto);
            }
            return dtoList;
        }

        public static IEnumerable<SelectListItem> GetWorkoutNamesForDropDown()
        {
            IEnumerable<SelectListItem> WorkoutNames = db.Workouts.Select(x => new SelectListItem()
            {
                Text = x.Name,
                Value=x.ID.ToString()
            }).ToList();
            return WorkoutNames;

        }

        public bool IsWorkoutExist(string name)
        {
            try
            {
                Workout workout = db.Workouts.FirstOrDefault(x => x.Name == name);
                if (workout != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public bool Delete(int ID)
        {
            try
            {
                Workout workout = db.Workouts.First(x => x.ID == ID);
                db.Workouts.Remove(workout);
                db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public void Update(Workout entity)
        {
            Workout workout = db.Workouts.FirstOrDefault(x => x.ID == entity.ID);
            workout.Name = entity.Name;
            workout.Calories = entity.Calories;
            workout.ActivityLevelID = entity.ActivityLevelID;
            workout.WorkoutCategoryID = entity.WorkoutCategoryID;
            db.SaveChanges();
        }
    }
}
