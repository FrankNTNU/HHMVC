using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class CommentDAO : HHContext
    {
        public void DeleteByMemberID(int ID)
        {
            List<Comment> comments = db.Comments.Where(x => x.MemberID == ID).ToList();
            db.Comments.RemoveRange(comments);
            db.SaveChanges();
        }

        public List<CommentDTO> GetAllComments()
        {
            List<CommentDTO> dtoList = new List<CommentDTO>();

            using (HealthHelperEntities db = new HealthHelperEntities())
            {
                var list = (from c in db.Comments
                            select new
                            {
                                ID = c.ID,
                                Title = c.Title,
                                Content = c.CommentContent,
                                TargetCommentTitle = c.Post.Title,
                                CategoryName = c.CommentCategory.Name,
                                AddDate = c.AddDate,
                                IsApproved = c.IsApproved
                            }).OrderBy(x => x.AddDate).ToList();
                foreach (var item in list)
                {
                    CommentDTO dto = new CommentDTO();
                    dto.ID = item.ID;
                    dto.Title = item.Title;
                    dto.CommentContent = item.Content;
                    dto.CategoryName = item.CategoryName;
                    dto.TargetCommentTitle = item.TargetCommentTitle;
                    dto.AddDate = item.AddDate;
                    dto.IsApproved = item.IsApproved;
                    dtoList.Add(dto);
                }
            }
            return dtoList;
        }
    }
}
