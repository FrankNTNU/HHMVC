using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class ProgramDAO : HHContext
    {



        public static double[]  GetInitialTargetWeights(int memberId)
        {
            double[] initNtarget = new double[2];
            Program program = db.Programs.FirstOrDefault(p => p.MemberID == memberId && p.StatusID == 1);
            if (program != null)
            {
                initNtarget[0] = program.InitialWeight;
                initNtarget[1] = program.TargetWeight;
            }
            return initNtarget;
        }

            public bool HasActiveProgram(int memberID)
        {
            return db.Programs.Any(p => p.MemberID == memberID && p.StatusID == 1);
        }

        public double GetSuccessRate()
        {
            var rate = ((double)db.Programs.Where(pr => pr.StatusID == 5).Count() / (double)db.Programs.Where(pr=>pr.StatusID!=1).Count())*100;

            return Math.Round(rate, 1);
        }

        public void RegisterProgram(Program p)
        {
            db.Programs.Add(p);
            db.SaveChanges();
        }

        public Program GetCurrentProgram(int memberID)
        {         
             return   db.Programs.FirstOrDefault(p => p.MemberID == memberID && p.StatusID == 1);
        }

        public void TerminateProgram(int programID)
        {
            if (db.Programs.Any(p => p.ID == programID))
            {
                db.Programs.FirstOrDefault(p => p.ID == programID).StatusID= 6;
                db.SaveChanges();

            }
        }

        public Program GetProgramByProgramID(int programID)
        {
            return db.Programs.FirstOrDefault(p => p.ID == programID);
        }
    }
}
