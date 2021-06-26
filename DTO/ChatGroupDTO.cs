using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DTO
{
    public class ChatGroupDTO {

        public string GroupName;

        public Tuple<int, int> WeightRange;

        public List<UserDetail> GroupMembers;
    }

}