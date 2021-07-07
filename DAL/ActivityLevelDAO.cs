using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace DAL
{
    public class ActivityLevelDAO : HHContext
    {
        public static IEnumerable<SelectListItem> GetActivityLevelsForDropDown()
        {
            IEnumerable<SelectListItem> activityLevels = db.ActivityLevels
                .Select(x => new SelectListItem()
                {
                    Text = x.Description,
                    Value = x.ID.ToString()
                }).ToList();
            return activityLevels;
        }
        public List<ActivityLevelDTO> GetLevels()
        {
            List<ActivityLevelDTO> activities = new List<ActivityLevelDTO>();
            var list = db.ActivityLevels.ToList();

            foreach (var item in list)
            {
                ActivityLevelDTO dto = new ActivityLevelDTO();
                dto.ID = item.ID;
                dto.Description = item.Description;
                activities.Add(dto);
            }
            return activities;
        }
    }
}
