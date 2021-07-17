using BLL;
using DAL;
using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UI.Models;

namespace UI.ViewModels
{
    public class ShowProgramViewModel
    {
        WeightLogBLL wlBLL = new WeightLogBLL();
        ProgramBLL pBLL = new ProgramBLL();
        MemberBLL mbBLL = new MemberBLL();
        private int _MemberID;
        private int _tDEE;
        private int _programMax;

        public ShowProgramViewModel(int memberID) {

            _MemberID = memberID;
            Member member = mbBLL.GetMemberByMemberID(_MemberID);
            MemberForDietDTO memberDto = new MemberForDietDTO(DateTime.Now.ToString(CDictionary.MMddyyyy))
            {
                MemberID = member.ID,
                Birthdate = member.Birthdate,
                ActivityLevelID = member.ActivityLevelID,
                Gender = member.Gender,
                Height = member.Height,
            };

            if (CurrProgram != null)
            {
                memberDto.ActivityLevelID = CurrProgram.ActivityLevelID;
                memberDto.Program = new ProgramDTO()
                {
                    Name = CurrProgram.Name,
                    StartDate = CurrProgram.StartDate,
                    EndDate = CurrProgram.EndDate,
                    TargetWeight = CurrProgram.TargetWeight,
                    InitialWeight = CurrProgram.InitialWeight
                };
                _tDEE =  (int)Math.Round(HealthCalculator.TDEE(memberDto, memberDto.Age, (decimal)MemberLatestWeight));
                _programMax = (int)HealthCalculator.GetProgramMaxCal(memberDto, memberDto.Program, memberDto.Age);

            }









        }
        public int ProgramMax { get { return _programMax; } }

        public int TDEE { get { return _tDEE; } }
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
