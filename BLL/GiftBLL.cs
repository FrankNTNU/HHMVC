using DAL;
using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class GiftBLL
    {
        GiftDAO giftDAO = new GiftDAO();
        public List<GiftDTO> GetGifts()
        {
            return giftDAO.GetGifts();
        }
        public bool AddGift(GiftDTO model)
        {
            Gift gift = new Gift();
            gift.Name = model.Name;
            gift.Image = model.Image;
            gift.Points = model.Points;
            gift.Quantity = model.Quantity;
            gift.AddDate = DateTime.Today;
            gift.EndDate = model.EndDate;
            gift.Store = model.Store; 
            giftDAO.AddGift(gift);
            return true;
        }
        public string UpdateGift(GiftDTO model)
        {
            model.AddDate = DateTime.Today;
            return giftDAO.UpdateGift(model);
        }
        public string DeleteGift(int ID)
        {
            return giftDAO.DeleteGift(ID);
        }

        public GiftDTO GetGift(int ID)
        {
            return giftDAO.GetGift(ID);
        }
        public void RemoveOneGift(int giftID)
        {
            giftDAO.RemoveOneGift(giftID);
        }

        public List<GiftDTO> GetGifts(string text)
        {
            return giftDAO.GetGifts(text);
        }

        public List<GiftDTO> GetGifts(bool isAscending)
        {
            return giftDAO.GetGifts(isAscending);
        }
    }
}
