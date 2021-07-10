using DAL;
using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class DietLogBLL
    {
        DietLogDAO dietLogDAO = new DietLogDAO();
        public void DeleteByMemberID(int ID)
        {
            dietLogDAO.DeleteByMemberID(ID);
        }
        public void Add(DietLogDTO entity)
        {
            DietLogDAO dao = new DietLogDAO();
            DietLog dietLog = new DietLog();
            dietLog.MemberID = entity.MemberID;
            dietLog.TimeOfDayID = entity.TimeOfDayID;
            dietLog.EditTime = entity.EditTime;
            dietLog.Portion = entity.Portion;
            dietLog.MealOptionID = entity.MealOptionID;
            dietLog.Date = entity.Date;
            dietLog.IsValid = entity.IsValid;

            dao.Add(dietLog);
        }
    }
}
