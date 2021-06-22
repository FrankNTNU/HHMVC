using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class GiftCartDAO : HHContext
    {
        int memberID = System.Web.HttpContext.Current.Session["ID"] == null ? 0 : (int)System.Web.HttpContext.Current.Session["ID"];
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
                    dto.IsExpired = item.EndDate < DateTime.Today;
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
                    giftCarts = db.GiftCarts.Where(x => x.MemberID == memberID && x.EndDate > DateTime.Now).OrderByDescending(x => x.EndDate).ToList();
                }
                else
                {
                    giftCarts = db.GiftCarts.Where(x => x.MemberID == memberID && x.EndDate > DateTime.Now).OrderBy(x => x.EndDate).ToList();
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
                    dto.IsExpired = item.EndDate < DateTime.Today;
                    dtoList.Add(dto);
                }
                return dtoList;
            }
        }

        public void DeleteCartsByMemberID(int userID)
        {
            List<GiftCart> carts = db.GiftCarts.Where(x => x.MemberID == userID).ToList();
            db.GiftCarts.RemoveRange(carts);
            db.SaveChanges();
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
                List<GiftCart> giftCarts = db.GiftCarts.Where(x => x.MemberID == memberID && x.Name.Contains(text) && x.EndDate > DateTime.Now).ToList();
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
                    dto.IsExpired = item.EndDate < DateTime.Today;
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
                    dto.IsExpired = item.EndDate < DateTime.Today;
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
