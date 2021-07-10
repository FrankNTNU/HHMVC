using DAL;
using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class GiftCartBLL
    {
        int memberID = System.Web.HttpContext.Current.Session["ID"] == null ? 0 : (int)System.Web.HttpContext.Current.Session["ID"];
        GiftCartDAO giftCartDAO = new GiftCartDAO();
        public List<GiftCartDTO> GetGiftCarts(int userID)
        {
            return giftCartDAO.GetGiftCarts(userID);
        }
        public List<GiftCartDTO> GetGiftCarts(bool isAscending)
        {
            return giftCartDAO.GetGiftCarts(isAscending);
        }
        public List<GiftCartDTO> GetGiftCarts(string text)
        {
            return giftCartDAO.GetGiftCarts(text);
        }
        public List<GiftCartDTO> GetGiftCarts()
        {
            return giftCartDAO.GetGiftCarts();
        }
        UserDAO userDAO = new UserDAO(); 

        public bool AddCart(GiftCartDTO model)
        {
            GiftCart cart = new GiftCart();
            cart.GiftID = model.GiftID;
            cart.Name = model.Name;
            cart.Store = model.Store;
            cart.MemberID = memberID;
            cart.Image = model.Image;
            cart.AddDate = DateTime.Today;
            cart.EndDate = model.EndDate;
            cart.Barcode = Guid.NewGuid().ToString("N").Substring(0, 17);
            giftCartDAO.AddCart(cart);
            userDAO.DeductPoints(memberID, model.Points);
            return true;
        }

        public string DeleteCart(int ID)
        {
            return giftCartDAO.DeleteCart(ID);
        }

        public bool IsSameItemExist(string name)
        {
            return giftCartDAO.IsSameItemExist(name);
        }
        public bool IsSameItemExist(int userID, int giftID) {
            return giftCartDAO.IsSameItemExist(userID, giftID);
        }

        internal void DeleteCartsByMemberID(int userID)
        {
            giftCartDAO.DeleteCartsByMemberID(userID);
        }
    }
}
