using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO;
using DAL;

namespace BLL
{
    public class MealBLL
    {
        public List<MealDetailDTO> GetMeals()
        {
            List<MealDetailDTO> dto = new List<MealDetailDTO>();
            MealDAO dao = new MealDAO();
            dto = dao.GetMeals();
            return dto;
        }
    }
}
