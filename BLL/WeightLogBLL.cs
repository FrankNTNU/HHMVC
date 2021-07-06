using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class WeightLogBLL
    {
        WeightLogDAO weightLogDAO = new WeightLogDAO();
        public void DeleteByMemberID(int ID)
        {
            weightLogDAO.DeleteByMemberID(ID);
        }

        public WeightLog GetLatestWeightByMemberID(int memberId)
        {
            return weightLogDAO.GetLatestWeightByMemberID(memberId);
        }

        public WeightLog GetLatestWeightByMemberIdPriorDate(int memberId, string date)
        {
            return weightLogDAO.GetLatestWeightByMemberIdPriorDate(memberId, date);
        }

        public void AddWeightLogViaProgramRegister(int memberID, int weight)
        {
            weightLogDAO.AddWeightLogViaProgramRegister(memberID, weight);
        }

        public double[] GetMonthlyWeights(int memberId, DateTime date)
        {
            return weightLogDAO.GetMonthlyWeights(memberId, date);

        }
    }
}
