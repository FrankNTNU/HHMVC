using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class GiftBLL
    {
        GiftDAO dao = new GiftDAO();
        public IQueryable<Gift> GetGiftByLevel()  //int? level
        {
            return dao.GetGiftByLevel();

        }
    }
}
