using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class DietLogBLL
    {
        DietLogDAO dietLogDAO = new DietLogDAO();
        public void DeleteByMemberID(int ID)
        {
            dietLogDAO.DeleteByMemberID(ID);
        }

    }
}
