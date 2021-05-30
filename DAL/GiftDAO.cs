using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class GiftDAO:HHContext
    {
        public List<GiftDTO> GetGift()
        {
            var gift = db.Gifts.ToList();
            List<GiftDTO> list = new List<GiftDTO>();
            
            foreach (var item in gift)
            {
                GiftDTO dto = new GiftDTO();
                dto.ID = item.ID;
                dto.Name = item.Name;
                dto.Image = item.Image;
                dto.Point = item.Points;
                list.Add(dto);
            }

            return list;
        }
        public void Add(Gift entity)
        {
            db.Gifts.Add(entity);
            db.SaveChanges();
        }
    }
}
