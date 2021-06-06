using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace DTO
{
    public class MealDetailDTO
    {
        public int ID { get; set; }
        [Required(ErrorMessage ="請輸入餐點名稱")]
        public string Name { get; set; }
        [Required(ErrorMessage = "請輸入餐點熱量")]
        public int Calories { get; set; }
        public List<TagCategoryDetailDTO> Tags { get; set; }
        public string MealOptionImage { get; set; }
        public HttpPostedFileBase MealOptionUpLoadImage { get; set; } // Uploaded image
        
        public string UnitName { get; set; }
        public TagCategoryDetailDTO TagCategoryDetail { get; set; }
        public string TagStringList { get; set; }
        public List<TagCategoryDetailDTO> TagList { get; set; }
        public string TagName { get; set; }
        public string TagImage { get; set; }
        public HttpPostedFileBase TagUpLoadImage { get; set; } // Uploaded image

        public NutrientDTO Nutrient { get; set; }
        public int NutrientID { get; set; }
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
