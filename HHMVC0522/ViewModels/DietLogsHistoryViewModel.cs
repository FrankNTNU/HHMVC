using DAL;
using BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DTO;
using System.Globalization;
using System.Web.Mvc;
using UI.Models;

namespace UI.ViewModels
{
    public class DietLogsHistoryViewModel
    {
        DietLogBLL dlBLL = new DietLogBLL();
        TimeOfDayBLL todBLL = new TimeOfDayBLL();
        CustomerMealBLL cmBLL = new CustomerMealBLL();
        MemberBLL mbBLL = new MemberBLL();


        private int _memberID;
        private int _dayDifference=0;
        private string _date = DateTime.Now.ToString(CDictionary.MMddyyyy);
        private bool _needDeleteEditOpts;
        private MemberHealthProfile _memberProfile; 

        public DietLogsHistoryViewModel(int memberID, bool needDeleteEditOpts ,bool needReport) {

            _memberID = memberID;
            _needDeleteEditOpts = needDeleteEditOpts;
            if (needReport)
            {
                _memberProfile = _GetMemberHealthProfile(_memberID, _date, needReport);
            }
           
            
        }
        private MemberHealthProfile _GetMemberHealthProfile(int memberId,string date, bool needReport)
        {
            Member member = mbBLL.GetMemberByMemberID(memberId);
            MemberForDietDTO mDto = new MemberForDietDTO(_date)
            {
                MemberID = member.ID,
                Birthdate = member.Birthdate,
                ActivityLevelID = member.ActivityLevelID,
                Gender = member.Gender,
                Height = member.Height,
            };
            MemberHealthProfile memberProfile = new MemberHealthProfile(mDto, date, needReport);
            return memberProfile;
        }

        public DietLogsHistoryViewModel(int memberID, string date, bool needDeleteEditOpts,  bool needReport)
        {

            _memberID = memberID;
            _dayDifference = GetDaysDifference(date);
            _date = date;
            _needDeleteEditOpts = needDeleteEditOpts;
            if (needReport)
            {
                _memberProfile = _GetMemberHealthProfile(_memberID, _date, needReport);
            }


        }
        public bool NeedDeleteEditOpts { get { return _needDeleteEditOpts; } }


        private int GetDaysDifference(string date)
        {

            DateTime theDate =DateTime.ParseExact(date, CDictionary.MMddyyyy, CultureInfo.InvariantCulture);
            int diff = (theDate-DateTime.Today ).Days;

            return diff;
        }

        public int MemberID { get { return _memberID; } }
        //public IQueryable<TimesOfDay> TimesOfDays { get { return todBLL.GetTimeOfDays(); } }


        public IEnumerable<SelectListItem> TimesOfDayForDropDown { get { return todBLL.GetTimesOfDayForDropDown(); } }

        public int DayDifference { get { return _dayDifference; } }
        public string Date { get { return _date; } }

        public List<DietLogViewModel> DietLogsOfTheDate { get {
                return GetDietLogsByDate(_memberID, _date);
            } }



        private List<DietLogViewModel> GetDietLogsByDate(int memberId, string date)
        {
            List<DietLogViewModel> dietLogVModels = new List<DietLogViewModel>();

            foreach (DietLog dl in dlBLL.GetDietLogsByDate(memberId,  date))
            {
                dietLogVModels.Add(new DietLogViewModel(dl));
            }

            foreach (TempCustomerMealOption dl in cmBLL.GetCustomerMealByDate(memberId, date))
            {
                DietLogViewModel model = new DietLogViewModel(dl);
                dietLogVModels.Add(model);
            }
            dietLogVModels.OrderBy(vm => vm.TimesOfDayID).ThenBy(vm => vm.MealTotalGainedCal);
            return dietLogVModels;
        }


        public MemberHealthProfile MemberHealthProfile { get { return _memberProfile; } }



    }
}


