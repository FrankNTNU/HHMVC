using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class WorkoutDTO
    {
        public List<WorkoutDetailDTO> Workouts { get; set; }
        public List<ActivityLevelDetailDTO> ActivityLevels { get; set; }
        public List<WorkoutCategoryDetailDTO> Categories { get; set; }

    }
}
