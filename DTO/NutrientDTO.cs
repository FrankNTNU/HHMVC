using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
   public class NutrientDTO
    {
        public int ID { get; set; }
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
