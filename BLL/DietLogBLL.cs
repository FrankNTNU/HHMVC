using DAL;
using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class 
        
        
        DietLogBLL
    {
        DietLogDAO dietLogDAO = new DietLogDAO();
        
        public void DeleteByMemberID(int ID)
        {
            dietLogDAO.DeleteByMemberID(ID);
        }

        public IQueryable<DietLog> GetDietLogs(int memberID)   //TODO Add CustomerUploade
        {
            return dietLogDAO.GetDietLogsByMemberID(memberID);
        }




        public List<int> GetMonthlyGainedCals(int memberId, DateTime date)
        {
            return dietLogDAO.GetMonthlyGainedCals(memberId, date);
        }

        public IQueryable<DietLog> GetDietLogsByDate(int memberID, string date)
        {
            


            return dietLogDAO.GetDietLogsByDate(memberID, date);
        }

        public IQueryable<DietLog> GetDietLogsToday(int memberID)
        {
            //return dietLogDAO.GetDietLogsByDate(memberID, DateTime.Now.ToString(CDictionary.MMddyyyy));
            return GetDietLogsByDate(memberID, DateTime.Now.ToString(CDictionary.MMddyyyy));

        }
        public IQueryable<DietLog> GetDietLogsByKeyword(string keyword, int memberID)
        {
            
            return dietLogDAO.GetDietLogsByKeyword(keyword, memberID);
        }

        public void Add(DietLogDTO dto)
        {
            DietLog entity = new DietLog();
            entity.Date = dto.Date;
            entity.MemberID = dto.MemberID;
            entity.TimeOfDayID = dto.TimeOfDayID;
            entity.MealOptionID = dto.MealOptionID;
            entity.Portion = dto.Portion;
            entity.IsValid = (dto.Date == DateTime.Now.ToString(CDictionary.MMddyyyy)) ? true : false;
            entity.EditTime = DateTime.Now.ToString(CDictionary.yyyyMMddHHmmss);
            dietLogDAO.AddToDietLog(entity);
        }
        public void Add_Hui(DietLogDTO entity)
        {
            DietLogDAO dao = new DietLogDAO();
            DietLog dietLog = new DietLog();
            dietLog.MemberID = entity.MemberID_Hui;
            dietLog.TimeOfDayID = entity.TimeOfDayID_Hui;
            dietLog.EditTime = entity.EditTime_Hui;
            dietLog.Portion = entity.Portion_Hui;
            dietLog.MealOptionID = entity.MealOptionID_Hui;
            dietLog.Date = entity.Date_Hui;
            dietLog.IsValid = entity.IsValid;

            dao.Add(dietLog);
        }


        public void EditDietLogByID(DietLogEditDTO editDto)
        {
            dietLogDAO.EditDietLogByID(editDto);
        }

        public void DeleteDietLog(int id, bool IsCustomUploaded)
        {
            dietLogDAO.DeleteDietLog(id, IsCustomUploaded);
        }




        //=================
        public double GetGainedCalByDate(string date, int memberID)
        {

            return dietLogDAO.GetGainedCalByDate(date, memberID);
        }

        public double GetGainedCalByDateTimeOfDay(int memberID , string date, int timeOfDayID)
        {
            return dietLogDAO.GetGainedCalByDateTimeOfDay(memberID, date, timeOfDayID);
        }

        public double GetPriorAvgGainedCalTODByDate(string date, int memberId, int todId)
        {
            return dietLogDAO.GetPriorAvgGainedCalTODByDate(date, memberId, todId);

        }

        public double GetPriorAvgGainedCalByDate(string date, int memberId)
        {
            return dietLogDAO.GetPriorAvgGainedCalByDate(date, memberId);

        }
        public int[] GetNearby7DaysGainedCal(int memberID, string date)
        {
            return dietLogDAO.GetNearby7DaysGainedCal(memberID, date);

        }

        //public int[] GetNearby7DaysTdeeOrProgramMax(int memberID, string date)
        //{
        //    return dietLogDAO.GetNearby7DaysTdeeOrProgramMax(memberID, date);
        //}

        public int[] Past7DaysGainedCalFromDate(int memberID, string date)  //todo customMealCal
        {
            return dietLogDAO.Past7DaysGainedCalFromDate(memberID, date);
        }




        //===================================================
        public double GetGainedProtein(int memberID, string date)
        {
            return dietLogDAO.GetGainedProtein(memberID, date);
        }

        public double GetGainedCarbs(int memberID, string date)
        {
            return dietLogDAO.GetGainedCarbs(memberID, date);
        }

        public double GetGainedFat(int memberID, string date)
        {
            return dietLogDAO.GetGainedFat(memberID, date);
        }

        public double GetGainedNa(int memberID, string date)
        {
            return dietLogDAO.GetGainedNa(memberID, date);
        }

        public double GetGainedSugar(int memberID, string date)
        {
            return dietLogDAO.GetGainedSugar(memberID, date);
        }
    }
}
