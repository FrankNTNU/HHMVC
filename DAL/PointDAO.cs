using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class PointDAO : HHContext
    {
        public bool[] GetMonthlyIsSuccess(int memberId, DateTime date)
        {
            int howManyDays = DateTime.DaysInMonth(date.Year, date.Month);
            DateTime firstDate = new DateTime(date.Year, date.Month, 1);
            DateTime lastDate = new DateTime(date.Year, date.Month, howManyDays);

            bool[] isSuccessArr = new bool[howManyDays];
           
            var successDatesDay =db.Points.Where(p=>p.MemberID == memberId).Where(p=>DbFunctions.TruncateTime(p.GetPointsDateTime)>= firstDate && DbFunctions.TruncateTime(p.GetPointsDateTime)<= lastDate).Where(p=>p.StatusID == 8)
                .Select(p=> p.GetPointsDateTime.Day);
            foreach (var day in successDatesDay)
            {
                isSuccessArr[day - 1] = true;
            }
            return isSuccessArr;

           
            
        }
    }
}
