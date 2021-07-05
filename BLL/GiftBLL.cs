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
            gift.IsPremium = model.IsPremium;
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

        public List<GiftDTO> GetSearchResult(string name, int sortingMethod, bool isPremium)
        {
            List<Gift> list = giftDAO.GetSearchResult(name, sortingMethod).Where(x => x.EndDate >= DateTime.Today).ToList();
            if (isPremium) list = list.Where(x => x.IsPremium == isPremium).ToList();
            List<GiftDTO> dtoList = new List<GiftDTO>();
            foreach (Gift item in list)
            {
                GiftDTO dto = new GiftDTO();
                dto.ID = item.ID;
                dto.Name = item.Name;
                dto.AddDate = item.AddDate;
                dto.EndDate = item.EndDate;
                dto.Store = item.Store;
                dto.IsPremium = item.IsPremium;
                dto.Points = item.Points;
                dto.Image = item.Image;
                dto.Quantity = item.Quantity;
                dtoList.Add(dto);
            }
            return dtoList;
        }
    }
}
