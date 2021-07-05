using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class CustomerMealDAO:HHContext
    {
        public void Add(TempCustomerMealOption entity)
        {
            db.TempCustomerMealOptions.Add(entity);
            db.SaveChanges();
        }

        public IQueryable<TempCustomerMealOption> GetCustomerMealByDate(int memberId, string date)
        {
           
                return db.TempCustomerMealOptions.Where(cm => cm.MemberID == memberId && cm.Date == date && cm.StatusID != CDictionary.StatusAprove).OrderBy(dl => dl.TimeOfDayID).ThenByDescending(dl => dl.MealOption.Calories).Select(dl => dl);

        }
    }
}
