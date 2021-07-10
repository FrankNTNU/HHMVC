﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO;
using DAL;

namespace BLL
{
    public class MealBLL
    {
        public List<MealDetailDTO> GetMeals()
        {
            List<MealDetailDTO> dto = new List<MealDetailDTO>();
            MealDAO dao = new MealDAO();
            dto = dao.GetMeals();
            return dto;
        }
        public List<MealDetailDTO> GetNutrientAndMeals()
        {
            List<MealDetailDTO> dto = new List<MealDetailDTO>();
            MealDAO dao = new MealDAO();
            dto = dao.GetNutrientAndMeals();
            return dto;
        }

        public MealDetailDTO GetMeals(int ID)
        {
            MealDetailDTO dto = new MealDetailDTO();
            MealDAO dao = new MealDAO();
            dto = dao.GetMeals(ID);
            return dto;
        }
        
        public MealDetailDTO GetMealWithTags(int ID)
        {
            MealDetailDTO dto = new MealDetailDTO();
            MealDAO dao = new MealDAO();
            dto = dao.GetMealAndTags(ID);
            return dto;
        }
        public List<MealDetailDTO> GetOnlyMeals()
        {
            List<MealDetailDTO> dto = new List<MealDetailDTO>();
            MealDAO dao = new MealDAO();
            dto = dao.GetOnlyMeals();

            return dto;
        }
        public int Add(MealDetailDTO dto)
        {
            NutrientDAO ndao = new NutrientDAO();
            MealOption meal = new MealOption();
            Nutrient nutrient = new Nutrient();
            NutrientDTO ndto = new NutrientDTO();
            MealDetailDTO mdto = new MealDetailDTO();
            mdto.Name = dto.Name;
            mdto.Calories = dto.Calories;
            mdto.MealOptionImage = dto.MealOptionImage;
            mdto.UnitName = dto.UnitName;
            ndto.Fat = dto.Fat;
            ndto.Protein = dto.Protein;
            ndto.Carbs = dto.Carbs;
            ndto.Sugar = dto.Sugar;
            ndto.VitA = dto.VitA;
            ndto.VitB = dto.VitB;
            ndto.VitC = dto.VitC;
            ndto.VitD = dto.VitD;
            ndto.VitE = dto.VitE;
            ndto.Na = dto.Na;
            ndto.Potassium= dto.Potassium;
            ndto.Calcium = dto.Calcium;
            ndto.Icon = dto.Icon;

            int mealID = ndao.nutrientAdd(ndto);
            mdto.NutrientID = mealID;
            mdto.Fat = ndto.Fat;
            mdto.Protein = ndto.Protein;
            mdto.Carbs = ndto.Carbs;
            mdto.Sugar = ndto.Sugar;
            mdto.VitA = ndto.VitA;
            mdto.VitB = ndto.VitB;
            mdto.VitC = ndto.VitC;
            mdto.VitD = ndto.VitD;
            mdto.VitE = ndto.VitE;
            mdto.Na = ndto.Na;
            mdto.Potassium = ndto.Potassium;
            mdto.Calcium = ndto.Calcium;
            mdto.Icon = ndto.Icon;
            return MealAdd(mdto);
        }
        public int MealAdd(MealDetailDTO entity)
        {
            MealDAO dao = new MealDAO();
            MealOption meal = new MealOption();
            meal.Name = entity.Name;
            meal.Calories = entity.Calories;
            meal.NutrientID = entity.NutrientID;
            meal.Image = entity.MealOptionImage;
            meal.UnitName = entity.UnitName;
            meal.IsVisable = "True";
            return dao.Add(meal);
        }
        public void DeleteMeal(int ID)
        {
            MealDAO dao = new MealDAO();
            dao.DeleteMeal(ID);
        }
        public void HiddenMeal(int ID)
        {
            MealDAO dao = new MealDAO();
            dao.HiddenMeal(ID);
        }
        public void DisplayMeal(int ID)
        {
            MealDAO dao = new MealDAO();
            dao.DisplayMeal(ID);
        }
        public string UpdateMeal(MealDetailDTO dto)
        {
            MealDAO dao = new MealDAO();
            return dao.UpdateMeal(dto);
        }
        public bool checkMealName(string userInput)
        {
            MealDAO dao = new MealDAO();
            bool check = dao.checkMealName(userInput);
            return check;
        }
    }
}
