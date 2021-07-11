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
            dto.Potassium = (float)nutrient.Potassium;


            return dto;
        }
        public NutrientDTO GetNutrientUseNurtientID(int ID)
        {
            NutrientDTO dto = new NutrientDTO();
            Nutrient nutrient = db.Nutrients.First(x => x.ID == ID);
          //  dto.ID = nutrient.ID;

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
            dto.Potassium = (float)nutrient.Potassium;
            dto.Calcium = (float)nutrient.Calcium;
            dto.Icon = (float)nutrient.Iron;


            return dto;
        }
        public int nutrientAdd(NutrientDTO entity)
        {
            Nutrient nutrient = new Nutrient();
            nutrient.Fat = entity.Fat;
            nutrient.Protein = entity.Protein;
            nutrient.Carbs = entity.Carbs;
            nutrient.Sugar = entity.Sugar;
            nutrient.VitA = entity.VitA;
            nutrient.VitB = entity.VitB;
            nutrient.VitC = entity.VitC;
            nutrient.VitD = entity.VitD;
            nutrient.VitE = entity.VitE;
            nutrient.Na = entity.Na;
            nutrient.Potassium = entity.Potassium;
            return NutrientAddToDB(nutrient);
        }
        public int NutrientAddToDB(Nutrient nutrient)
        {
            try
            {
                db.Nutrients.Add(nutrient);
                db.SaveChanges();
                return nutrient.ID;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public void DeleteNutrient(int ID)
        {
            try
            {
                Nutrient nutrient = db.Nutrients.First(x => x.ID == ID);
                db.Nutrients.Remove(nutrient);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void UpdateNutrient(MealDetailDTO entity)
        {
            Nutrient nutrient = db.Nutrients.First(x => x.ID == entity.NutrientID);
            nutrient.Fat = entity.Fat;
            nutrient.Protein = entity.Protein;
            nutrient.Carbs = entity.Carbs;
            nutrient.Sugar = entity.Sugar;
            nutrient.VitA = entity.VitA;
            nutrient.VitB = entity.VitB;
            nutrient.VitC = entity.VitC;
            nutrient.VitD = entity.VitD;
            nutrient.VitE = entity.VitE;
            nutrient.Na = entity.Na;
            nutrient.Potassium = entity.Potassium;
            nutrient.Calcium = entity.Calcium;
            nutrient.Iron = entity.Icon;
            db.SaveChanges();
        }
    }
}