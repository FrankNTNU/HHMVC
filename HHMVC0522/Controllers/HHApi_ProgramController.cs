using BLL;
using DAL;
using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using UI.Models;
using UI.ViewModels;

namespace UI.Controllers
{
    public class HHApi_ProgramController : Controller
    {
        // GET: HHProgram
        ProgramBLL pBLL = new ProgramBLL();
        WeightLogBLL wlBLL = new WeightLogBLL();
        MemberBLL mBLL = new MemberBLL();
        public PartialViewResult RegisterProgram(Program program)
        {
            Thread.Sleep(1000);
            Member member = mBLL.GetMemberByMemberID(program.MemberID);
             MemberForDietDTO mDto = new MemberForDietDTO(DateTime.Now.ToString(CDictionary.MMddyyyy))
             {
                 MemberID = member.ID,
                 Birthdate = member.Birthdate,
                 ActivityLevelID = member.ActivityLevelID,
                 Gender = member.Gender,
                 Height = member.Height,

             };
            var tDEEToBe = HealthCalculator.TDEE(mDto, mDto.Age, program.InitialWeight);
            ProgramDTO programToBe = new ProgramDTO() {
                Name = program.Name,
                StartDate = program.StartDate,
                EndDate = program.EndDate,
                TargetWeight = program.TargetWeight,
                InitialWeight = program.InitialWeight

            };
            var programMaxToBe  = HealthCalculator.GetProgramMaxCal(mDto, programToBe, mDto.Age);
            if (programMaxToBe / tDEEToBe < (decimal)0.8)
            {  //這樣減脂太難了啦
                return RegisterOrShowProgramByMemberID(mDto.MemberID);
            }
            else { 
            pBLL.RegisterProgram(program);
            wlBLL.AddWeightLogViaProgramRegister(program.MemberID, program.InitialWeight);
            mBLL.UpdateActivityLevel(program.MemberID, program.ActivityLevelID);
            return RegisterOrShowProgramByMemberID(program.MemberID);
            }
        }

        public PartialViewResult TerminateProgram(int programId)
        {
            Program target = pBLL.GetProgramByProgramID(programId);
            pBLL.TerminateProgram(target.ID);
            return RegisterOrShowProgramByMemberID(target.MemberID);
        }

        public PartialViewResult RegisterOrShowProgramByMemberID(int memberId)
        {
            PartialViewResult result = PartialView("_RegisterProgramPartial", new RegisterProgramViewModel(memberId));
            if (pBLL.HasActiveProgram(memberId))
            {
                result = PartialView("_ShowProgramPartial", new ShowProgramViewModel(memberId));
            }

            return result;
        }



    }
    }