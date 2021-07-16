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
        ProgramBLL pBLL = new ProgramBLL();
        ViewModelGenerator vmGenerator = new ViewModelGenerator();


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
            Program program = pBLL.GetCurrentProgram(memberId);
            MemberForDietDTO mDto = new MemberForDietDTO(_date)
            {
                MemberID = member.ID,
                Birthdate = member.Birthdate,
                ActivityLevelID = member.ActivityLevelID,
                Gender = member.Gender,
                Height = member.Height,
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
                    InitialWeight = program.InitialWeight
                };
            }

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
        public DateTime DateDT { get {
                return  DateTime.ParseExact(Date, CDictionary.MMddyyyy, CultureInfo.InvariantCulture);
                ;
            } }

        public List<DietLogViewModel> DietLogsOfTheDate { get {
                return vmGenerator.GetDietLogsByDate(_memberID, _date);
            } }



     

        public MemberHealthProfile MemberHealthProfile { get { return _memberProfile; } }



    }
}


