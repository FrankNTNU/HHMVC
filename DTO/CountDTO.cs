using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class CountDTO
    {
        public int UserCount { get; set; }
        public int UnapprovedCommentCount { get; set; }
        public int UnapprovedPostCount { get; set; }
        public int NewMemberCount { get; set; }
        public int UnderstockedGifts { get; set; }
        public int DietLogCount { get; set; }
        public int WeightLogCount { get; set; }
        public int WorkoutLogCount { get; set; }
        public int GiftCartCount { get; set; }
        public int PostCount { get; set; }
        public int CommentCount { get; set; }
        public List<string> PastSixMonths { get; set; }
        public List<int> HalfOfYearDietLogCount { get; set; }
        public List<int> HalfOfYearWeightLogCount { get; set; }
        public List<int> HalfOfYearMemberCount { get; set; }
        public List<int> HalfOfYearMemberSum { get; set; }

    }
}
