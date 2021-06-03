using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class NutrientDAO : HHContext
    {
        public NutrientDTO GetNutrient(int ID)
        {
            MealOption meal = db.MealOptions.First(x => x.ID == ID);
            NutrientDTO dto = new NutrientDTO();
            int nutrientID = (int)meal.NutrientID;

            Nutrient nutrient = db.Nutrients.First(x => x.ID == nutrientID);
            dto.ID = nutrient.ID;

            //把資料庫資料匯入清單
            dto.Fat = (float)nutrient.Fat;
            dto.Protein = (float)nutrient.Protein;
            dto.Carbs = (float)nutrient.Carbs;
            dto.Sugar = (float)nutrient.Sugar;
            dto.VitA = (float)nutrient.VitA;
            dto.VitB = (float)nutrient.VitB;
            dto.VitC = (float)nutrient.VitC;
            dto.VitD = (float)nutrient.VitD;
            dto.VitE = (float)nutrient.VitE;
            dto.Na = (float)nutrient.Na;
            dto.K = (float)nutrient.Potassium;


            return dto;
        }
    }
}
