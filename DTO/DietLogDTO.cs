using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class DietLogDTO
    {
        public int ID { get; set; }
        public int MemberID_Hui { get; set; }
        public int TimeOfDayID_Hui { get; set; }
        public string EditTime_Hui { get; set; }
        public float Portion_Hui { get; set; }
        public int MealOptionID_Hui { get; set; }
        public string Date_Hui { get; set; }
        public bool IsValid { get; set; }
        private MixedDietLogDTO _mixedDietLogDTO;
        public DietLogDTO(MixedDietLogDTO dto) {

            _mixedDietLogDTO = dto;
        }
        public DietLogDTO()
        {

        }
        public int MemberID { get { return _mixedDietLogDTO.MemberID; } }
        public int TimeOfDayID { get { return _mixedDietLogDTO.TimeOfDayID; } }
        public double Portion { get { return _mixedDietLogDTO.Portion; } }
        public int MealOptionID { get { return _mixedDietLogDTO.MealOptionID; } }
        public string Date { get { return _mixedDietLogDTO.Date; } }
    }
}
