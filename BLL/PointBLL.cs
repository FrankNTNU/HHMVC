using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class PointBLL
    {
        PointDAO dao = new PointDAO();

        public bool[] GetMonthlyIsSuccess(int memberId, DateTime date)
        {
            return dao.GetMonthlyIsSuccess(memberId, date);
        }
    }
}
