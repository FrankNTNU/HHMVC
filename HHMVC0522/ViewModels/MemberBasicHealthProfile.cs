using BLL;
using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using UI.Models;
using System.Globalization;
using DAL;

namespace UI.ViewModels
{
    public class MemberBasicHealthProfile
    {
        WeightLogBLL wBLL = new WeightLogBLL();
        private MemberForDietDTO _memberForDiet;
        private decimal _weight;
        public MemberBasicHealthProfile( MemberForDietDTO mDto) {
            _memberForDiet = mDto;
            _weight =(decimal)wBLL.GetLatestWeightByMemberIdPriorDate(_memberForDiet.MemberID, _memberForDiet.date).Weight;

    }
        public int  MemberID { get { return _memberForDiet.MemberID; } }
     public ProgramDTO CurrProgram { get { return _memberForDiet.Program; } }


        public int TDEE { get { return (int)Math.Round(HealthCalculator.TDEE(_memberForDiet, _memberForDiet.Age, _weight)); } }  //TODO coordinate with Enchi
      
        public int ProgramMaxCalOrTDEE
        {
            get
            {

                if (CurrProgram != null)
                {
                    return (int)HealthCalculator.GetProgramMaxCal(_memberForDiet, CurrProgram, _memberForDiet.Age);
                }

                return TDEE;
            }
        }


    }
}