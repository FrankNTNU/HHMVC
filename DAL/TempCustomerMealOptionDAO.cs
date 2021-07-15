using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class TempCustomerMealOptionDAO:HHContext
    {
        public List<TempCustomerMealOptionDTO> GetTempCustomerMealOptions()
        {
            StatusDAO statusDAO = new StatusDAO();
            var tempCustomerMealOptionList = db.TempCustomerMealOptions.ToList();
            List<TempCustomerMealOptionDTO> dtoList = new List<TempCustomerMealOptionDTO>();
            foreach(var item in tempCustomerMealOptionList)
            {
                TempCustomerMealOptionDTO dto = new TempCustomerMealOptionDTO();
                dto.ID = item.ID;
                dto.Name = item.Name;
                dto.Calories = (decimal)item.Calories;
                dto.ImagePath = item.ImagePath;
                dto.StatusID = item.StatusID;
                dto.MealOptionID = item.MealOptionID;
                dto.MemberID = item.MemberID;
                dto.EditTime = item.EditTime;
                dto.Date = item.Date;
                dto.IsValid = item.IsValid;
                dto.TimeOfDayID = item.TimeOfDayID;
                dto.Portion = (float)item.Portion;
                dto.UnitName = item.UnitName;
                dto.stausName = statusDAO.GetStatus(dto.StatusID);
                dtoList.Add(dto);
            }

            return dtoList;
        }
        public List<TempCustomerMealOptionDTO> GetTempCustomerMealOptionsNotProceeded()
        {
            StatusDAO statusDAO = new StatusDAO();
            var tempCustomerMealOptionList = db.TempCustomerMealOptions.Where(x=>x.StatusID==2).ToList();
            List<TempCustomerMealOptionDTO> dtoList = new List<TempCustomerMealOptionDTO>();
            foreach (var item in tempCustomerMealOptionList)
            {
                TempCustomerMealOptionDTO dto = new TempCustomerMealOptionDTO();
                dto.ID = item.ID;
                dto.Name = item.Name;
                dto.Calories = (decimal)item.Calories;
                dto.ImagePath = item.ImagePath;
                dto.StatusID = item.StatusID;
                dto.MealOptionID = item.MealOptionID;
                dto.MemberID = item.MemberID;
                dto.EditTime = item.EditTime;
                dto.Date = item.Date;
                dto.IsValid = item.IsValid;
                dto.TimeOfDayID = item.TimeOfDayID;
                dto.Portion = (float)item.Portion;
                dto.UnitName = item.UnitName;
                dto.stausName = statusDAO.GetStatus(dto.StatusID);
                dtoList.Add(dto);
            }

            return dtoList;
        }
        public List<TempCustomerMealOptionDTO> GetTempCustomerMealOptionsPassed()
        {
            StatusDAO statusDAO = new StatusDAO();
            var tempCustomerMealOptionList = db.TempCustomerMealOptions.Where(x => x.StatusID == 13).ToList();
            List<TempCustomerMealOptionDTO> dtoList = new List<TempCustomerMealOptionDTO>();
            foreach (var item in tempCustomerMealOptionList)
            {
                TempCustomerMealOptionDTO dto = new TempCustomerMealOptionDTO();
                dto.ID = item.ID;
                dto.Name = item.Name;
                dto.Calories = (decimal)item.Calories;
                dto.ImagePath = item.ImagePath;
                dto.StatusID = item.StatusID;
                dto.MealOptionID = item.MealOptionID;
                dto.MemberID = item.MemberID;
                dto.EditTime = item.EditTime;
                dto.Date = item.Date;
                dto.IsValid = item.IsValid;
                dto.TimeOfDayID = item.TimeOfDayID;
                dto.Portion = (float)item.Portion;
                dto.UnitName = item.UnitName;
                dto.stausName = statusDAO.GetStatus(dto.StatusID);
                dtoList.Add(dto);
            }

            return dtoList;
        }
        public List<TempCustomerMealOptionDTO> GetTempCustomerMealOptionsFailed()
        {
            StatusDAO statusDAO = new StatusDAO();
            var tempCustomerMealOptionList = db.TempCustomerMealOptions.Where(x => x.StatusID == 12).ToList();
            List<TempCustomerMealOptionDTO> dtoList = new List<TempCustomerMealOptionDTO>();
            foreach (var item in tempCustomerMealOptionList)
            {
                TempCustomerMealOptionDTO dto = new TempCustomerMealOptionDTO();
                dto.ID = item.ID;
                dto.Name = item.Name;
                dto.Calories = (decimal)item.Calories;
                dto.ImagePath = item.ImagePath;
                dto.StatusID = item.StatusID;
                dto.MealOptionID = item.MealOptionID;
                dto.MemberID = item.MemberID;
                dto.EditTime = item.EditTime;
                dto.Date = item.Date;
                dto.IsValid = item.IsValid;
                dto.TimeOfDayID = item.TimeOfDayID;
                dto.Portion = (float)item.Portion;
                dto.UnitName = item.UnitName;
                dto.stausName = statusDAO.GetStatus(dto.StatusID);
                dtoList.Add(dto);
            }

            return dtoList;
        }
        public void auditNotPassed(int ID)
        {
            TempCustomerMealOption tempCustomerMealOption = db.TempCustomerMealOptions.First(x => x.ID == ID);
            tempCustomerMealOption.StatusID = 12;
            db.SaveChanges();
        }
        public AuditPassDTO GetTempCustomerMealOption(int ID)
        {
            AuditPassDTO dto = new AuditPassDTO();
            TempCustomerMealOptionDTO tempDTO = new TempCustomerMealOptionDTO();
            TempCustomerMealOption temp = db.TempCustomerMealOptions.First(x => x.ID == ID);
            //tempDTO.ID = temp.ID;
            //tempDTO.Name = temp.Name;
            //tempDTO.Calories = (decimal)temp.Calories;
            //tempDTO.ImagePath = temp.ImagePath;
            //tempDTO.StatusID = temp.StatusID;
            //tempDTO.MealOptionID = temp.MealOptionID;
            //tempDTO.MemberID = temp.MemberID;
            //tempDTO.EditTime = temp.EditTime;
            //tempDTO.Date = temp.Date;
            //tempDTO.IsValid = temp.IsValid;
            //tempDTO.TimeOfDayID = temp.TimeOfDayID;
            //tempDTO.Portion = (float)temp.Portion;
            //tempDTO.UnitName = temp.UnitName;
            //dto.tempCustomerMealOptionList.Add(tempDTO);

            dto.ID = temp.ID;
            dto.Name = temp.Name;
            dto.Calories = (float)temp.Calories;
            dto.ImagePath = temp.ImagePath;
            dto.StatusID = temp.StatusID;
            dto.MealOptionID = temp.MealOptionID;
            dto.MemberID = temp.MemberID;
            dto.EditTime = temp.EditTime;
            dto.Date = temp.Date;
            dto.IsValid = temp.IsValid;
            dto.TimeOfDayID = temp.TimeOfDayID;
            dto.Portion = (float)temp.Portion;
            dto.UnitName = temp.UnitName;

            return dto;
        }
        public void Update(int ID,int mealID)
        {
            TempCustomerMealOption tempCustomerMealOption = db.TempCustomerMealOptions.First(x => x.ID == ID);
            tempCustomerMealOption.MealOptionID = mealID;
            tempCustomerMealOption.StatusID = 13;
            db.SaveChanges();
        }

    }
}
