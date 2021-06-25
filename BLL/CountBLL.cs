using DAL;
using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class CountBLL
    {
        CountDAO countDAO = new CountDAO();
        public CountDTO GetCounts()
        {
            int count = countDAO.GetUserCount();
            CountDTO countDTO = new CountDTO();
            countDTO.UserCount = count;
            return countDTO;
        }
    }
}
