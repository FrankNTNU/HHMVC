using BLL;
using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UI.ViewModels;

namespace UI.Controllers
{
    public class HHApi_ProgramController : Controller
    {
        // GET: HHProgram
        ProgramBLL pBLL = new ProgramBLL();
        WeightLogBLL wlBLL = new WeightLogBLL();
        public PartialViewResult RegisterProgram(Program program)
        {
            pBLL.RegisterProgram(program);
            wlBLL.AddWeightLogViaProgramRegister(program.MemberID, program.InitialWeight);   //todo
            //UpdateActivity Levey //todo
            return RegisterOrShowProgramByMemberID(program.MemberID);
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