using BLL;
using DAL;
using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using UI.Models;

namespace UI.ViewModels
{
    public class MealStyleListViewModel
    {
        MealTagBLL mtBLL = new MealTagBLL();
        MemberBLL mBLL = new MemberBLL();
        DietLogBLL dlBLL = new DietLogBLL();
        private MemberBasicHealthProfile _memberBasicHealthProfile;
        private MealStyleViewModel _mealTagInfo;
        //private IQueryable<MealOption> _styleMeals;
        private double _todayGainedCals;

        public MealStyleListViewModel(int memberId, int mealStyleId, Program program)
        {

            Member member = mBLL.GetMemberByMemberID(memberId);
            MemberForDietDTO mDto = new MemberForDietDTO(DateTime.Now.ToString(CDictionary.MMddyyyy))
            {
                MemberID = member.ID,
                Birthdate = member.Birthdate,
                ActivityLevelID = member.ActivityLevelID,
                Gender = member.Gender,
                Height = member.Height
            };
            if (program != null)
            {
                mDto.ActivityLevelID = program.ActivityLevelID;
                mDto.Program = new ProgramDTO()
                {
                    Name = program.Name,
                    StartDate = program.StartDate,
                    EndDate = program.EndDate,
                    TargetWeight = program.TargetWeight,
                    InitialWeight = program.InitialWeight,

                };
            }




            _memberBasicHealthProfile = new MemberBasicHealthProfile(mDto);
            _mealTagInfo = new MealStyleViewModel(mealStyleId, memberId);
            _todayGainedCals = dlBLL.GetGainedCalByDate(DateTime.Now.ToString(CDictionary.MMddyyyy), memberId);

        }



        public MemberBasicHealthProfile MemberProfile { get { return _memberBasicHealthProfile; } }

        public MealStyleViewModel TheMealStyle { get { return _mealTagInfo; } }

        public double TodayGainedCals { get { return _todayGainedCals; } }


        public IQueryable<MealTagCategory> TagsCategories { get { return mtBLL.GetAllTags(); } }

        public int RemainActiveCal {get{ return MemberProfile.ProgramMaxCalOrTDEE - (int)TodayGainedCals; } }
    }
}