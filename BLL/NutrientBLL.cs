using DAL;
using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class NutrientBLL
    {
        public void Delete(int ID)
        {
            NutrientDAO dao = new NutrientDAO();
            dao.DeleteNutrient(ID);
        }
        public NutrientDTO GetNutrientUseNurtientID(int ID)
        {
            NutrientDTO dto = new NutrientDTO();
            NutrientDAO dao = new NutrientDAO();
            dto = dao.GetNutrientUseNurtientID(ID);
            return dto;
        }
        public void UpdateNutrient(MealDetailDTO dto)
        {
            NutrientDAO dao = new NutrientDAO();
            dao.UpdateNutrient(dto);
        }
    }
}
