using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class MealTagBLL
    {
        MealTagDAO dao = new MealTagDAO();

        public IQueryable<MealTagCategory> GetAllTags()
        {

            return dao.GetAllTags();
        }

        public MealTagCategory GetTagByID(int id)
        {

            return dao.GetTagByID(id);
        }

    }
}
