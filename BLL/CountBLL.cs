using DAL;
using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class CountBLL
    {
        CountDAO countDAO = new CountDAO();
        public CountDTO GetCounts()
        {
            CountDTO countDTO = new CountDTO();
            countDTO.UserCount = countDAO.GetUserCount();
            countDTO.UnapprovedCommentCount = countDAO.GetUnapprovedCommentCount();
            countDTO.UnapprovedPostCount = countDAO.GetUnapprovedPostCount();
            countDTO.NewMemberCount = countDAO.GetNewMemberCount();
            countDTO.UnderstockedGifts = countDAO.GetUnderstockedGiftCount();
            countDTO.DietLogCount = countDAO.GetDietLogCount();
            countDTO.WeightLogCount = countDAO.GetWeightLogCount();
            countDTO.WorkoutLogCount = countDAO.GetWorkoutLogCount();
            countDTO.GiftCartCount = countDAO.GetGiftCartCount();
            countDTO.CommentCount = countDAO.GetCommentCount();
            countDTO.PostCount = countDAO.GetPostCount();
            countDTO.PastSixMonths = countDAO.GetPastSixMonthNames();
            countDTO.HalfOfYearDietLogCount = countDAO.GetHalfOfYearDietLogCount();
            countDTO.HalfOfYearWeightLogCount = countDAO.GetHalfOfYearWorkoutCount();
            countDTO.HalfOfYearMemberCount = countDAO.GetHalfOfYearMemberCount();
            countDTO.HalfOfYearMemberSum = countDAO.GetHalfOfYearMemberSum();
            return countDTO;
        }
    }
}
