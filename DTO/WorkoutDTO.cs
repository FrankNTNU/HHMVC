using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace DTO
{
    public class WorkoutDTO
    {
        public List<WorkoutItemDTO> WorkoutItems { get; set; }
        public List<ActivityLevelDTO> ActivityLevels { get; set; }
        public List<WorkoutCategoryDTO> Categories { get; set; }
        public List<WorkoutDetailDTO> Workouts { get; set; }
        public List<ActivityLevelDetailDTO> ActivityLevels { get; set; }

        public IEnumerable<SelectListItem> ActivityLevelNames { get; set; }
    }
}
