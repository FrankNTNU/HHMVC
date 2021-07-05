using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class MealTagDAO: HHContext
    {
        public IQueryable<MealTagCategory> GetAllTags()
        {
            var q = from mtc in db.MealTagCategories
                    select mtc;
            return q;
        }

        public MealTagCategory GetTagByID(int id)
        {
         
            return db.MealTagCategories.FirstOrDefault(mtc => mtc.ID == id);
        }
    }
}
