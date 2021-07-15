using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace DAL
{ 
    public class WorkoutCategoryDAO : HHContext
    {
        public List<WorkoutCategoryDTO> GetCategories()
        {
            List<WorkoutCategory> categories = db.WorkoutCategories.OrderBy(x => x.ID).ToList();
            List<WorkoutCategoryDTO> dtoList = new List<WorkoutCategoryDTO>();
            foreach (var item in categories)
            {
                WorkoutCategoryDTO dto = new WorkoutCategoryDTO();
                dto.ID = item.ID;
                dto.Name = item.Name;
                dtoList.Add(dto);
            }
            return dtoList;
        }

        public List<WorkoutCategoryDTO> GetCategories(string text)
        {
            List<WorkoutCategory> categories = db.WorkoutCategories.Where(x => x.Name.Contains(text)).ToList();
            List<WorkoutCategoryDTO> dtoList = new List<WorkoutCategoryDTO>();
            foreach (var item in categories)
            {
                WorkoutCategoryDTO dto = new WorkoutCategoryDTO();
                dto.ID = item.ID;
                dto.Name = item.Name;
                dtoList.Add(dto);
            }
            return dtoList;
        }

        public bool Add(WorkoutCategory entity)
        {
            try
            {
                WorkoutCategoryDTO categoryDTO = new WorkoutCategoryDTO();
                //var q = from p in db.WorkoutCategories
                //                             select new
                //                             {
                //                                 categoryNames=p.Name
                //                             };
                //categoryDTO.categoryNames.Add(q.ToString());
                db.WorkoutCategories.Add(entity);
                db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public WorkoutCategoryDTO GetWorkoutCayWithID(int ID)
        {
            WorkoutCategoryDTO dto = new WorkoutCategoryDTO();
            WorkoutCategory category = db.WorkoutCategories.First(x => x.ID == ID);
            if (category != null)
            {
                dto.ID = category.ID;
                dto.Name = category.Name;
            }
            return dto;
        }

        public void Update(WorkoutCategory entity)
        {
            try
            {              
                WorkoutCategory category = db.WorkoutCategories.First(x => x.ID == entity.ID);
                if (category != null)
                {
                    category.Name = entity.Name;
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public bool Delete(int ID)
        {
            WorkoutCategory category = db.WorkoutCategories.First(x => x.ID == ID);
            db.WorkoutCategories.Remove(category);
            db.SaveChanges();
            return true;
        }

        public bool HasWorkouts(int ID)
        {
            Workout workout = db.Workouts.FirstOrDefault(x => x.WorkoutCategoryID == ID);
            if (workout != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool IsCategoryExist(string text)
        {
            try
            {
                WorkoutCategory category = db.WorkoutCategories.FirstOrDefault(x => x.Name == text);
                if (category != null)
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

        public static IEnumerable<SelectListItem> GetWorkoutCategoriesForDropDown()
        {
            IEnumerable<SelectListItem> workoutCategories = db.WorkoutCategories.Select(x => new SelectListItem()
            {
                Text = x.Name,
                Value = x.ID.ToString()
            }).ToList();
            return workoutCategories;
        }
    }
}
