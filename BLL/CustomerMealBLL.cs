using DAL;
using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class CustomerMealBLL
    {
        CustomerMealDAO dao = new CustomerMealDAO();

        public void Add(TempCustomerMealDTO dto)
        {
            TempCustomerMealOption entity = new TempCustomerMealOption();
            entity.Name = dto.Name;
            entity.Calories = dto.Calories;
            entity.StatusID = CDictionary.StatusPendingReview;
            entity.UnitName = dto.UnitName;
            //entity.ImagePath; //todo
            entity.Date = dto.Date;
            entity.MemberID = dto.MemberID;
            entity.TimeOfDayID = dto.TimeOfDayID;
            entity.Portion = dto.Portion;
            entity.ImagePath = "defaultCustomeMeal.jpg";
            entity.IsValid = (dto.Date == DateTime.Now.ToString(CDictionary.MMddyyyy)) ? true : false;
            entity.EditTime = DateTime.Now.ToString(CDictionary.yyyyMMddHHmmss);
            dao.Add(entity);
        }

        public IQueryable<TempCustomerMealOption> GetCustomerMealByDate(int memberId, string date)
        {
            return dao.GetCustomerMealByDate(memberId, date);
        }


    }
}
