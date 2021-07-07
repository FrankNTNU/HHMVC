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
    public class WorkoutCategoryBLL
    {
        WorkoutCategoryDAO dao = new WorkoutCategoryDAO();
        public bool Add(WorkoutCategoryDTO entity)
        {
            WorkoutCategory workoutCategory = new WorkoutCategory();
            workoutCategory.Name = entity.Name;
            return dao.Add(workoutCategory);
        }
        public List<WorkoutCategoryDTO> GetCategories()
        {
            List<WorkoutCategoryDTO> dtoList = new List<WorkoutCategoryDTO>();
            dtoList = dao.GetCategories();
            return dtoList;
        }
        public List<WorkoutCategoryDTO> GetCategories(string text)
        {
            List<WorkoutCategoryDTO> dtoList = new List<WorkoutCategoryDTO>();
            dtoList = dao.GetCategories(text);
            return dtoList;
        }

        public void Update(WorkoutCategoryDTO entity)
        {
            WorkoutCategory category = new WorkoutCategory();
            category.ID = entity.ID;
            category.Name = entity.Name;
            dao.Update(category);
        }

        public WorkoutCategoryDTO GetWorkoutCayWithID(int ID)
        {
            return dao.GetWorkoutCayWithID(ID);
        }

        public bool IsCategoryExist(string text)
        {
            return dao.IsCategoryExist(text);
        }

        public bool HasWorkouts(int ID)
        {
            return dao.HasWorkouts(ID);
        }

        public bool Delete(int ID)
        {
            return dao.Delete(ID);
        }
        WorkoutDAO workoutDAO = new WorkoutDAO();
        public List<WorkoutItemDTO> GetWorkouts(int ID)
        {
            List<WorkoutItemDTO> dtoList = new List<WorkoutItemDTO>();
            dtoList = workoutDAO.GetWorkouts(ID);
            return dtoList;
        }

        public static IEnumerable<SelectListItem> GetWorkoutCategoriesForDropDown()
        {
            return WorkoutCategoryDAO.GetWorkoutCategoriesForDropDown();
        }
    }
}
