using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DTO
{
    public class ChatGroupDTO {

        public string GroupName { get; set; }

        public Tuple<int, int> WeightRange { get; set; }

        public Difficulty Difficulty { get; set; }

        public List<UserDetail> GroupMembers { get; set; }
    }


    public enum Difficulty
    {
        None,
        Easy,
        Normal,
        Hard
    }
}