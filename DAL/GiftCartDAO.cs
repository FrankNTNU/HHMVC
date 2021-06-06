using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class GiftCartDAO
    {
        public List<GiftCartDTO> GetGiftCarts(int userID)
        {
            List<GiftCartDTO> dtoList = new List<GiftCartDTO>();
            using (HealthHelperEntities db = new HealthHelperEntities())
            {
                List<GiftCart> giftCarts = db.GiftCarts.Where(x => x.MemberID == userID).OrderBy(x => x.EndDate).ToList();
                foreach (var item in giftCarts)
                {
                    GiftCartDTO dto = new GiftCartDTO();
                    dto.ID = item.ID;
                    dto.GiftID = item.GiftID;
                    dto.Name = item.Name;
                    dto.Store = item.Store;
                    dto.Image = item.Image;
                    dto.MemberID = item.MemberID;
                    dto.Barcode = item.Barcode;
                    dto.BarcodeImage = item.BarcodeImage;
                    dto.AddDate = item.AddDate;
                    dto.EndDate = item.EndDate;
                    dtoList.Add(dto);
                }
                return dtoList;
            }
        }
        public List<GiftCartDTO> GetGiftCarts(bool isAscending)
        {
            List<GiftCartDTO> dtoList = new List<GiftCartDTO>();
            using (HealthHelperEntities db = new HealthHelperEntities())
            {
                List<GiftCart> giftCarts = new List<GiftCart>();
                if (isAscending)
                {
                    giftCarts = db.GiftCarts.Where(x => x.MemberID == UserStatic.UserID).OrderByDescending(x => x.EndDate).ToList();
                }
                else
                {
                    giftCarts = db.GiftCarts.Where(x => x.MemberID == UserStatic.UserID).OrderBy(x => x.EndDate).ToList();
                }
                foreach (var item in giftCarts)
                {
                    GiftCartDTO dto = new GiftCartDTO();
                    dto.ID = item.ID;
                    dto.GiftID = item.GiftID;
                    dto.Name = item.Name;
                    dto.Store = item.Store;
                    dto.Image = item.Image;
                    dto.MemberID = item.MemberID;
                    dto.Barcode = item.Barcode;
                    dto.BarcodeImage = item.BarcodeImage;
                    dto.AddDate = item.AddDate;
                    dto.EndDate = item.EndDate;
                    dto.Duration = item.AddDate.ToString("yyyy/MM/dd") + " 到 " + item.EndDate.ToString("yyyy/MM/dd");
                    dtoList.Add(dto);
                }
                return dtoList;
            }
        }

        public bool IsSameItemExist(int userID, int giftID)
        {
            GiftCart cart;
            using (HealthHelperEntities db = new HealthHelperEntities()) {
                cart = db.GiftCarts.FirstOrDefault(x => x.MemberID == userID && x.GiftID == giftID);
            }
            return cart != null;
        }

        public List<GiftCartDTO> GetGiftCarts(string text)
        {
            List<GiftCartDTO> dtoList = new List<GiftCartDTO>();
            using (HealthHelperEntities db = new HealthHelperEntities())
            {
                List<GiftCart> giftCarts = db.GiftCarts.Where(x => x.MemberID == UserStatic.UserID && x.Name.Contains(text)).ToList();
                foreach (var item in giftCarts)
                {
                    GiftCartDTO dto = new GiftCartDTO();
                    dto.ID = item.ID;
                    dto.GiftID = item.GiftID;
                    dto.Name = item.Name;
                    dto.Store = item.Store;
                    dto.Image = item.Image;
                    dto.MemberID = item.MemberID;
                    dto.Barcode = item.Barcode;
                    dto.AddDate = item.AddDate;
                    dto.EndDate = item.EndDate;
                    dto.Duration = item.AddDate.ToString("yyyy/MM/dd") + " 到 " + item.EndDate.ToString("yyyy/MM/dd");
                    dtoList.Add(dto);
                }
                return dtoList;
            }
        }
        public List<GiftCartDTO> GetGiftCarts()
        {
            List<GiftCartDTO> dtoList = new List<GiftCartDTO>();
            using (HealthHelperEntities db = new HealthHelperEntities())
            {
                List<GiftCart> giftCarts = db.GiftCarts.OrderByDescending(x => x.EndDate).ToList();
                foreach (var item in giftCarts)
                {
                    GiftCartDTO dto = new GiftCartDTO();
                    dto.ID = item.ID;
                    dto.GiftID = item.GiftID;
                    dto.Name = item.Name;
                    dto.Store = item.Store;
                    dto.Image = item.Image;
                    dto.MemberID = item.MemberID;
                    dto.MemberName = item.Member.Name;
                    dto.Barcode = item.Barcode;
                    dto.BarcodeImage = item.BarcodeImage;
                    dto.AddDate = item.AddDate;
                    dto.EndDate = item.EndDate;
                    dtoList.Add(dto);
                }
                return dtoList;
            }
        }

        public bool IsSameItemExist(string name)
        {
            using(HealthHelperEntities db = new HealthHelperEntities())
            {
                GiftCart cart = db.GiftCarts.FirstOrDefault(x => x.Name == name);
                return cart != null;
            }
        }
        
        //public GiftCartDTO GetGiftCart(int ID)
        //{
        //    GiftCartDTO dto = new GiftCartDTO();
        //    using (HealthHelperEntities db = new HealthHelperEntities())
        //    {
        //        GiftCart cart = db.GiftCarts.FirstOrDefault(x => x.ID == ID);
        //        dto.Name = cart.Name;
        //        dto.Image = cart.Image;
        //    }
        //    return dto;
        //}

        public void AddCart(GiftCart cart)
        {
            using (HealthHelperEntities db = new HealthHelperEntities())
            {
                db.GiftCarts.Add(cart);
                db.SaveChanges();
            }
        }
        public string DeleteCart(int ID)
        {
            string oldImagePath = "";
            using (HealthHelperEntities db = new HealthHelperEntities())
            {
                GiftCart gift = db.GiftCarts.FirstOrDefault(x => x.ID == ID);
                oldImagePath = gift.Image;
                db.GiftCarts.Remove(gift);
                db.SaveChanges();
            }
            return oldImagePath;
        }
    }
}
