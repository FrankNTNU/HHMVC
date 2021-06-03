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
                dto.Quantity =(int)item.Quantity;
                dto.AddDate = (DateTime)item.AddDate;
                dto.EndDate = (DateTime)item.EndDate;
                //dto.Deadline = item.Deadline;
                list.Add(dto);
            }

            return list;
        }
        public GiftDTO GetGift(int ID)
        {
            using (HealthHelperEntities db = new HealthHelperEntities())
            {
                Gift gift = db.Gifts.First(x => x.ID == ID);
                GiftDTO dto = new GiftDTO();
                dto.ID = gift.ID;
                dto.Name = gift.Name;
                dto.Image = gift.Image;
                dto.Point = gift.Points;
                dto.Quantity = (int)gift.Quantity;
                dto.AddDate = (DateTime)gift.AddDate;
                dto.EndDate = (DateTime)gift.EndDate;
                //dto.Deadline = gift.Deadline;

                return dto;
            }
        }
        public void Add(Gift entity)
        {
            db.Gifts.Add(entity);
            db.SaveChanges();
        }
        public void Delete(int id)
        {
            Gift gift = db.Gifts.First(x => x.ID == id);
            db.Gifts.Remove(gift);
            db.SaveChanges();
        }
        public string Update(GiftDTO entity)
        {
            Gift gift = db.Gifts.First(x => x.ID == entity.ID);
            gift.Name = entity.Name;
            gift.Points = entity.Point;
            string oldImagePass = gift.Image;
            gift.Image = entity.Image;
            gift.Quantity = entity.Quantity;
            gift.AddDate = entity.AddDate;
            gift.EndDate = entity.EndDate;
            db.SaveChanges();
            return oldImagePass;
        }
    }
}
