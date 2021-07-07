
using DAL;
using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace BLL
{
    public class WaterLogBLL
    {
        WaterLogDAO dao = new WaterLogDAO();


        public void UpdateWaterLog(WaterLog waterLogAmountToAdded)
        {
            dao.UpdateWaterLog(waterLogAmountToAdded);

        }

        public int[] GetWeeklyWaterLogs(int memberId, string date)
        {
            return dao.GetWeeklyWaterLogs(memberId, date);
        }

        public int[] GetCurrentWeekWaterLogs(int memberId)
        {
            return GetWeeklyWaterLogs(memberId, DateTime.Now.ToString(CDictionary.MMddyyyy));
        }
        public int[][] GetWeeklySuggestedWaterLogRanges(int memberId, string date)
        {
            return dao.GetWeeklySuggestedWaterLogRanges(memberId, date);
        }
    }
}
