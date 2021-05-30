using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class MealDetailDTO
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int Calories { get; set; }
        public List<TagCategoryDetailDTO> Tags { get; set; }
        public byte[] Image { get; set; }
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
        public float K { get; set; }

    }
}
