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
    }
}
