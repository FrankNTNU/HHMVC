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
        public List<MealDetailDTO> GetOnlyNutrient()
        {
            List<MealDetailDTO> dto = new List<MealDetailDTO>();
            NutrientDAO dao = new NutrientDAO();
            dto = dao.GetOnlyNutrient();
            return dto;
        }
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
