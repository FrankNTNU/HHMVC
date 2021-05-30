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
        GiftDAO dao = new GiftDAO();

        public List<GiftDTO> GetGift()
        {
            List<GiftDTO> dto = new List<GiftDTO>();
            dto = dao.GetGift();
            return dto;
        }
        public void Add(GiftDTO dto)
        {
            Gift gift = new Gift();
            gift.Name = dto.Name;
            gift.Image = dto.Image;
            gift.Points = dto.Point;
            dao.Add(gift);
        }
    }
}
