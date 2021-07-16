using BLL;
using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using UI.Models;
using UI.ViewModels;

namespace DTO
{
    public class CreateDietLogViewModel
    {
        TimeOfDayBLL todBLL = new TimeOfDayBLL();
        MealTagBLL mtBLL = new MealTagBLL();
        ProgramBLL pBLL = new ProgramBLL();
        DietLogBLL dlBLL = new DietLogBLL();
        MemberBLL mBLL = new MemberBLL();
        ViewModelGenerator vmGenerator = new ViewModelGenerator();
        private string _today = DateTime.Now.ToString(CDictionary.MMddyyyy);

        private MemberHealthProfile _memberProfile;
        //private DietLogsHistoryViewModel _todayDietLogs;
        public CreateDietLogViewModel(MemberForDietDTO mDto)
        {
            _memberProfile = new MemberHealthProfile(mDto, _today, false);
            //_todayDietLogs = new DietLogsHistoryViewModel(_memberProfile.MemberID, _today, false, false);

        }
        public MemberHealthProfile MemberHealthProfile { get { return _memberProfile; } }

        public IQueryable<MealTagCategory> tagsCategories { get { return mtBLL.GetAllTags(); } }

        public IQueryable<TimesOfDay> TimesOfDays { get { return todBLL.GetTimeOfDays(); } }


        public MixedDietLogDTO[] TempDietLogs { get; set; }
        public string DateOfToday { get { return _today; } }

        //public DietLogsHistoryViewModel TodayDietLogs { get { return _todayDietLogs; } }
        
             public List<DietLogViewModel> DietLogsOfTheDate
        {
            get
            {
                return vmGenerator.GetDietLogsByDate(_memberProfile.MemberID, _today);
            }
        }
    }
}


