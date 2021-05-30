using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class MemberDTO
    {
        public List<MemberDetailDTO> Members { get; set; }
        public List<StatusDetailDTO> Statuses { get; set; }
        public List<ActivityLevelDetailDTO> ActivityLevels { get; set; }
    }
}
