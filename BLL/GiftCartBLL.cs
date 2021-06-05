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

        public bool AddCart(GiftDTO model)
        {
            GiftCart cart = new GiftCart();
            cart.Name = model.Name;
            cart.Store = model.Store;
            cart.MemberID = UserStatic.UserID;
            cart.Image = model.Image;
            cart.AddDate = DateTime.Today;
            cart.EndDate = model.EndDate;
            cart.Barcode = Guid.NewGuid().ToString("N").Substring(0, 17);
            giftCartDAO.AddCart(cart);
            userDAO.DeductPoints(UserStatic.UserID, model.Points);
            return true;
        }

        public string DeleteCart(int ID)
        {
            return giftCartDAO.DeleteCart(ID);
        }

        public GiftCartDTO GetGiftCart(int ID)
        {
            return giftCartDAO.GetGiftCart(ID);
        }

        public bool IsSameItemExist(string name)
        {
            return giftCartDAO.IsSameItemExist(name);
        }
    }
}
