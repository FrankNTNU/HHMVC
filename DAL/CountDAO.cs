using System;
using System.Collections.Generic;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class CountDAO : HHContext
    {
        public int GetUserCount() => db.Members.Count();     

        public int GetUnapprovedCommentCount()
            => db.Comments.Where(x => x.IsApproved == false).Count();      

        public int GetUnapprovedPostCount()
            => db.Posts.Where(x => x.IsApproved == false).Count();

        public int GetUnapprovedDietLogCount()
            => db.TempCustomerMealOptions.Count();

        public int GetNewMemberCount()
        {
            DateTime lastWeek = DateTime.Today.AddDays(-7);
            return db.Members.Where(x => x.JoinDate <= lastWeek).Count();
        }

        public int GetUnderstockedGiftCount() 
            => db.Gifts.Where(x => x.Quantity < 10).Count();

        public int GetDietLogCount() => db.DietLogs.Count();
        public int GetWeightLogCount() => db.WeightLogs.Count();
        public int GetWorkoutLogCount() => db.WorkoutLogs.Count();
        public int GetGiftCartCount() => db.GiftCarts.Count();
        public int GetPostCount() => db.Posts.Count();
        public int GetCommentCount() => db.Comments.Count();
        
        public List<string> GetPastSixMonthNames()
        {
            List<string> namesOfMonths = new List<string>();
            for (int i = 5; i >= 0; i--)
            {
                string currentMonth = DateTime.Today.AddMonths(-i).Month + "月";
                namesOfMonths.Add(currentMonth);
            }
            return namesOfMonths;
        }

        public List<int> GetHalfOfYearDietLogCount()
        {
            List<int> dietLogCounts = new List<int>();
            for (int i = 5; i >= 0; i--)
            {
                string currentMonth = DateTime.Today.AddMonths(-i).Month.ToString();
                if (currentMonth.Length == 1) currentMonth = "0" + currentMonth;
                int num = db.DietLogs.Where(x => x.Date.Substring(0, 2) == currentMonth).Count();
                dietLogCounts.Add(num);
            }
            return dietLogCounts;
        }
        public List<int> GetHalfOfYearWorkoutCount()
        {
            List<int> workoutCounts = new List<int>();
            for (int i = 5; i >= 0; i--)
            {
                int currentMonth = DateTime.Today.AddMonths(-i).Month;
                int num = db.WorkoutLogs.Where(x => x.EditTime.Month == currentMonth).Count();
                workoutCounts.Add(num);
            }
            return workoutCounts;
        }
        public List<int> GetHalfOfYearMemberCount()
        {
            List<int> memebrCounts = new List<int>();
            for (int i = 5; i >= 0; i--)
            {
                int currentMonth = DateTime.Today.AddMonths(-i).Month;
                int num = db.Members.Where(x => x.JoinDate.Month == currentMonth).Count();
                memebrCounts.Add(num);
            }
            return memebrCounts;
        }
        public List<int> GetHalfOfYearMemberSum()
        {
            List<int> memberSum = new List<int>();
            for (int i = 5; i >= 0; i--)
            {
                DateTime currentMonth = DateTime.Today.AddMonths(-i);
                int num = db.Members.Where(x => x.JoinDate <= currentMonth).Count();
                memberSum.Add(num);
            }
            return memberSum;
        }
    }
}
