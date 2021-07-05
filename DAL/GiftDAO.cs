using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class GiftDAO:HHContext
    {
        public IQueryable<Gift> GetGiftByLevel()  //int? level
        {
            return db.Gifts.Select(g => g);
        
        }
    }
}
