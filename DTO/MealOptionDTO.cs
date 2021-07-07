using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class MealOptionDTO
    {
        public MealOptionDTO() { 
        
        }


        public int ID { get; set; }
        public string Name { get; set; }
        public double Calories { get; set; }
        public byte[] Image { get; set; }
       // public Nullable<int> NutrientID { get; set; }
        public string UnitName { get; set; }



    }
}
