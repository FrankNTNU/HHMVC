using BLL;
using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace UI.ViewModels
{
    public class RegisterProgramViewModel
    {

        WeightLogBLL wlBLL = new WeightLogBLL();
        ProgramBLL pBLL = new ProgramBLL();
        private int _MemberID;
        public RegisterProgramViewModel(int memberID) {

            _MemberID = memberID;
        }


        public int MemberID { get { return _MemberID; } }

        public IEnumerable<SelectListItem> ActivityLevelsSelectListItem { get { return ActivityLevelBLL.GetActivityLevelsForDropDown(); } }


        public double SuccessRate { get { return pBLL.GetSuccessRate(); } }

        public double MemberLatestWeight
        {
            get
            {
                double weight = 0;
                WeightLog weightRecord = wlBLL.GetLatestWeightByMemberID(_MemberID);
                if (weightRecord != null)
                {
                    weight = weightRecord.Weight;
                }
                return weight;
            }
        }
    }


    }
