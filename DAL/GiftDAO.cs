using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class GiftDAO : HHContext
    {
        public List<GiftDTO> GetGifts()
        {
            List<GiftDTO> giftList = new List<GiftDTO>();
            using (HealthHelperEntities db = new HealthHelperEntities())
            {
                List<Gift> gifts = db.Gifts.ToList();
                foreach (var item in gifts)
                {
                    GiftDTO dto = new GiftDTO();
                    dto.ID = item.ID;
                    dto.Name = item.Name;
                    dto.Image = item.Image;
                    dto.Points = item.Points;
                    dto.Quantity = item.Quantity;
                    dto.AddDate = item.AddDate;
                    dto.EndDate = item.EndDate;
                    dto.Store = item.Store;
                    giftList.Add(dto);
                }
            }
            return giftList;
        }
        public void AddGift(Gift gift)
        {
            using (HealthHelperEntities db = new HealthHelperEntities())
            {
                db.Gifts.Add(gift);
                db.SaveChanges();
            }
        }

        public void RemoveOneGift(int giftID)
        {
            using (HealthHelperEntities db = new HealthHelperEntities())
            {
                Gift gift = db.Gifts.FirstOrDefault(x => x.ID == giftID);
                gift.Quantity--;
                db.SaveChanges();
            }
        }

        public List<GiftDTO> GetGifts(bool isAscending)
        {
            List<GiftDTO> dtoList = new List<GiftDTO>();
            using (HealthHelperEntities db = new HealthHelperEntities())
            {
                List<Gift> gifts = new List<Gift>();
                if (isAscending)
                {
                    gifts = db.Gifts.Where(x => x.EndDate > DateTime.Today).OrderBy(x => x.Points).ToList();
                }
                else
                {
                    gifts = db.Gifts.Where(x => x.EndDate > DateTime.Today).OrderByDescending(x => x.Points).ToList();
                }
                foreach (var item in gifts)
                {
                    GiftDTO dto = new GiftDTO();
                    dto.ID = item.ID;
                    dto.Name = item.Name;
                    dto.Points = item.Points;
                    dto.Quantity = item.Quantity;
                    dto.Image = item.Image;
                    
                    dtoList.Add(dto);
                }
            }
            return dtoList;
        }

       
        public List<GiftDTO> GetGifts(string text)
        {
            List<GiftDTO> dtoList = new List<GiftDTO>();
            using (HealthHelperEntities db = new HealthHelperEntities()) 
            {
                List<Gift> gifts = db.Gifts.Where(x => x.Name.Contains(text) && x.EndDate > DateTime.Today).ToList();
                foreach (var item in gifts)
                {
                    GiftDTO dto = new GiftDTO();
                    dto.ID = item.ID;
                    dto.Name = item.Name;
                    dto.Points = item.Points;
                    dto.Quantity = item.Quantity;
                    dto.Image = item.Image;
                    dtoList.Add(dto);
                }
            }
            return dtoList;
        }

        public GiftDTO GetGift(int ID)
        {
            using (HealthHelperEntities db = new HealthHelperEntities())
            {
                Gift gift = db.Gifts.FirstOrDefault(x => x.ID == ID);
                GiftDTO model = new GiftDTO();
                model.ID = ID;
                model.Name = gift.Name;
                model.Image = gift.Image;
                model.Points = gift.Points;
                model.Quantity = gift.Quantity;
                model.AddDate = gift.AddDate;
                model.EndDate = gift.EndDate;
                model.Store = gift.Store;
                return model;
            }
        }

        public string UpdateGift(GiftDTO model)
        {
            string oldImagePath = "";
            using (HealthHelperEntities db = new HealthHelperEntities())
            {
                Gift gift = db.Gifts.FirstOrDefault(x => x.ID == model.ID);
                gift.Name = model.Name;
                if(model.Image != null)
                {
                    oldImagePath = gift.Image;
                    gift.Image = model.Image;
                }
                gift.Points = model.Points;
                gift.Quantity = model.Quantity;
                gift.AddDate = model.AddDate;
                gift.EndDate = model.EndDate;
                gift.Store = model.Store;
                db.SaveChanges();
            }
            return oldImagePath;
        }
        public string DeleteGift(int ID)
        {
            string oldImagePath = "";
            using (HealthHelperEntities db = new HealthHelperEntities())
            {
                Gift gift = db.Gifts.FirstOrDefault(x => x.ID == ID);
                oldImagePath = gift.Image;
                db.Gifts.Remove(gift);
                db.SaveChanges();
            }
            return oldImagePath;
        }
    }
}
