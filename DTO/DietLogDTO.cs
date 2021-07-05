using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class DietLogDTO
    {
        private MixedDietLogDTO _mixedDietLogDTO;
        public DietLogDTO(MixedDietLogDTO dto) {

            _mixedDietLogDTO = dto;
        }

        public int MemberID { get { return _mixedDietLogDTO.MemberID; } }
        public int TimeOfDayID { get { return _mixedDietLogDTO.TimeOfDayID; } }
        public double Portion { get { return _mixedDietLogDTO.Portion; } }
        public int MealOptionID { get { return _mixedDietLogDTO.MealOptionID; } }
        public string Date { get { return _mixedDietLogDTO.Date; } }
    }
}
