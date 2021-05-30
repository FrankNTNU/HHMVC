using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class WorkoutCategoryDTO
    {
        public List<WorkoutCategoryDetailDTO> Categories { get; set; }
        public List<WorkoutDetailDTO> Workouts { get; set; }
    }
}
