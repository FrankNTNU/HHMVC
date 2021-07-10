using DAL;
using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class TempCustomerMealOptionBLL
    {
        public List<TempCustomerMealOptionDTO> GetTempCustomerMealOptions()
        {
            TempCustomerMealOptionDAO dao = new TempCustomerMealOptionDAO();
            return dao.GetTempCustomerMealOptions();
        }
        public List<TempCustomerMealOptionDTO> GetTempCustomerMealOptionsNotProceeded()
        {
            TempCustomerMealOptionDAO dao = new TempCustomerMealOptionDAO();
            return dao.GetTempCustomerMealOptionsNotProceeded();
        }
        public List<TempCustomerMealOptionDTO> GetTempCustomerMealOptionsPassed()
        {
            TempCustomerMealOptionDAO dao = new TempCustomerMealOptionDAO();
            return dao.GetTempCustomerMealOptionsPassed();
        }
        public List<TempCustomerMealOptionDTO> GetTempCustomerMealOptionsFailed()
        {
            TempCustomerMealOptionDAO dao = new TempCustomerMealOptionDAO();
            return dao.GetTempCustomerMealOptionsFailed();
        }
        public void auditNotPassed(int ID)
        {
            TempCustomerMealOptionDAO dao = new TempCustomerMealOptionDAO();
            dao.auditNotPassed(ID);
        }
        public AuditPassDTO GetTempCustomerMealOption(int ID)
        {
            TempCustomerMealOptionDAO dao = new TempCustomerMealOptionDAO();
            return  dao.GetTempCustomerMealOption(ID);
        }
        public void AddToDB(AuditPassDTO dto)
        {
            MealDetailDTO mealDetailDTO = new MealDetailDTO();
            mealDetailDTO.Name = dto.Name;
            mealDetailDTO.Calories = (int)dto.Calories;
            mealDetailDTO.MealOptionImage = dto.ImagePath;
            mealDetailDTO.UnitName = dto.UnitName;
            mealDetailDTO.Fat = dto.Fat;
            mealDetailDTO.Protein = dto.Protein;
            mealDetailDTO.Carbs = dto.Carbs;
            mealDetailDTO.Sugar = dto.Sugar;
            mealDetailDTO.VitA = dto.VitA;
            mealDetailDTO.VitB = dto.VitB;
            mealDetailDTO.VitC = dto.VitC;
            mealDetailDTO.VitD = dto.VitD;
            mealDetailDTO.VitE = dto.VitE;
            mealDetailDTO.Na = dto.Na;
            mealDetailDTO.Potassium = dto.Potassium;
            mealDetailDTO.Calcium = dto.Calcium;
            mealDetailDTO.Icon = dto.Icon;
            MealBLL mealBLL = new MealBLL();
            int mealID = mealBLL.Add(mealDetailDTO);  //return MealID

            DietLogDTO dietLogDTO = new DietLogDTO();
            DietLogBLL dietLogBLL = new DietLogBLL();
            dietLogDTO.MemberID = dto.MemberID;
            dietLogDTO.TimeOfDayID = dto.TimeOfDayID;
            dietLogDTO.EditTime = dto.EditTime;
            dietLogDTO.Portion = dto.Portion;
            dietLogDTO.MealOptionID = mealID;
            dietLogDTO.Date = dto.Date;
            dietLogDTO.IsValid = dto.IsValid;
            dietLogBLL.Add(dietLogDTO);

            TempCustomerMealOptionDAO dao = new TempCustomerMealOptionDAO();
            dao.Update(dto.ID,mealID);

        }
    }
}
