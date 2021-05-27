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

        public List<CommentDTO> GetUnapprovedComments()
        {
            return commentDAO.GetUnapprovedComments();
        }

        public List<CommentDTO> GetAllComments(int userID)
        {
            return commentDAO.GetAllComments(userID);
        }

        public void ApproveComment(int ID)
        {
            commentDAO.ApproveComment(ID);
        }

        public void DeleteComment(int ID)
        {
            commentDAO.DeleteComment(ID);
        }
    }
}
