using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class AuditPassDTO
    {
        public List<TempCustomerMealOptionDTO> tempCustomerMealOptionList { get; set; }
        public int ID { get; set; }
        public string ImagePath { get; set; }
        public int StatusID { get; set; }
        public List<StatusDetailDTO> statusList { get; set; }
        public string stausName { get; set; }


        //For DietLog
        public int MemberID { get; set; }
        public int TimeOfDayID { get; set; }
        public string EditTime { get; set; }
        public float Portion { get; set; }
        public int MealOptionID { get; set; }
        public string Date { get; set; }
        public bool IsValid { get; set; }
        //For MealOption
        public string Name { get; set; }
        public float Calories { get; set; }
        public string Image { get; set; }
        public int NutrientID { get; set; }
        public string UnitName { get; set; }
        public string IsVisable { get; set; }
        //For Nutrient
        public float Fat { get; set; }
        public float Protein { get; set; }
        public float Carbs { get; set; }
        public float Sugar { get; set; }
        public float VitA { get; set; }
        public float VitB { get; set; }
        public float VitC { get; set; }
        public float VitD { get; set; }
        public float VitE { get; set; }
        public float Na { get; set; }
        public float Potassium { get; set; }
        public float Calcium { get; set; }//鈣
        public float Icon { get; set; }//鐵
    }
}
