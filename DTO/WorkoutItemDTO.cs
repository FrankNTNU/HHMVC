using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace DTO
{
    public class WorkoutItemDTO
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int CategoryID { get; set; }
        public string CategoryName { get; set; }
        public double Calories { get; set; }
        public int ActivityLevelID { get; set; }
        public string ActivityLevel { get; set; }

        public IEnumerable<SelectListItem> ActivityLevelNames { get; set; }

        public IEnumerable<SelectListItem> WorkoutCategories { get; set; }
    }
}
