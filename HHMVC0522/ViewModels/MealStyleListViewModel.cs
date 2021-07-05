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
        private MemberBasicHealthProfile _memberBasicHealthProfile;
        private MealStyleViewModel _mealTagInfo;
        private IQueryable<MealOption> _styleMeals;


        public MealStyleListViewModel(int memberId, int mealStyleId)
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

            
            _memberBasicHealthProfile = new MemberBasicHealthProfile(mDto);
            _mealTagInfo = new MealStyleViewModel(mealStyleId, memberId);

        }

        

        public MemberBasicHealthProfile MemberProfile { get { return _memberBasicHealthProfile; } }

        public MealStyleViewModel TheMealStyle { get { return _mealTagInfo; } }


        public IQueryable<MealTagCategory> TagsCategories { get { return mtBLL.GetAllTags(); } }


    }
}