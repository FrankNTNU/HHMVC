using DAL;
using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class CommentBLL
    {
        CommentDAO commentDAO = new CommentDAO();

        
        public void DeleteByMemberID(int ID)
        {
            commentDAO.DeleteByMemberID(ID);
        }

        public List<CommentDTO> GetAllComments()
        {
            return commentDAO.GetAllComments();
        }
    }
}
