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
        public GiftDTO GetGift(int id)
        {
            GiftDTO dto = new GiftDTO();
            dto = dao.GetGift(id);
            return dto;
        }
        public void Add(GiftDTO dto)
        {
            Gift gift = new Gift();
            gift.Name = dto.Name;
            gift.Image = dto.Image;
            gift.Points = dto.Point;
            gift.Quantity = dto.Quantity;
            gift.AddDate = dto.AddDate;
            gift.EndDate = dto.EndDate;
            dao.Add(gift);
        }
        public void Delete(int id)
        {
            dao.Delete(id);
        }

        public string Update(GiftDTO dto)
        {
            //Gift gift = new Gift();
            //gift.ID = dto.ID;
            //gift.Name = dto.Name;
            //gift.Image = dto.Image;
            //gift.Points = dto.Point;
            //gift.Quantity = dto.Quantity;
            //gift.Deadline = dto.Deadline;
            
            return dao.Update(dto);
        }
    }
}
