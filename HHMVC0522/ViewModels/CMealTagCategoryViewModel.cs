using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace UI.ViewModels
{
    public class CMealTagCategoryViewModel
    {
        public MealTagCategory mealTagCategory { get; set; }
      
        public CMealTagCategoryViewModel()
        {
            mealTagCategory = new MealTagCategory();
        }
        public int ID { get { return mealTagCategory.ID; } set { mealTagCategory.ID = value; } }
        public string Name { get { return mealTagCategory.Name; } set { mealTagCategory.Name = value; } }
        public string Image { get { return mealTagCategory.Image; } set { mealTagCategory.Image = value; } }

        public HttpPostedFileBase photo { get; set; }
    }
}