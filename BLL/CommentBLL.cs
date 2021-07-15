using Azure.AI.TextAnalytics;
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
            List<CommentDTO> comments = commentDAO.GetAllComments();
            return comments;
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
        public bool UpdateComment(CommentDTO model)
        {
            commentDAO.UpdateComment(model);
            return true;
        }
        public void DeleteComment(int ID)
        {
            commentDAO.DeleteComment(ID);
        }

        public CommentDTO GetComment(int commentID)
        {
            return commentDAO.GetComment(commentID);
        }

        public void ReportComment(int commentID)
        {
            commentDAO.ReportComment(commentID);
        }
        public void ClearReport(int commentID)
        {
            commentDAO.ClearReport(commentID);
        }
        public static class CommentStatistics
        {
            public static int CommentCount { get => CommentDAO.GetCommentCount(); }
            public static int UnapprovedCount { get => CommentDAO.GetUnapprovedCount(); }
            public static int ReportedCount { get => CommentDAO.GetReportedCount(); }
            public static int NegativeCount { get => CommentDAO.GetNegativeCount(); }

        }

        public void HideComment(int commentID)
        {
            commentDAO.HideComment(commentID);
        }
    }
}
