using BLL;
using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace UI.ViewModels
{
    public class ShowProgramViewModel
    {
        WeightLogBLL wlBLL = new WeightLogBLL();
        ProgramBLL pBLL = new ProgramBLL();
        private int _MemberID;
        public ShowProgramViewModel(int memberID) {

            _MemberID = memberID;
        }


        public Program CurrProgram { get { return pBLL.GetCurrentProgram(_MemberID); } }

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
