using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class ProgramBLL
    {
        ProgramDAO dao = new ProgramDAO();



        public void RegisterProgram(Program p)
        {
            dao.RegisterProgram(p);
        }

        public bool HasActiveProgram(int memberID)
        {
            return dao.HasActiveProgram(memberID);
        }

        public Program GetCurrentProgram(int memberID)
        {
            return dao.GetCurrentProgram(memberID);

        }

        public Program GetProgramByProgramID(int programID)
        {
            return dao.GetProgramByProgramID(programID);

        }

        public void TerminateProgram(int programID)
        {
             dao.TerminateProgram(programID);
        }

        public static double[] GetInitialTargetWeights(int memberId)
        {
            return ProgramDAO.GetInitialTargetWeights(memberId);
        }
    }
}
