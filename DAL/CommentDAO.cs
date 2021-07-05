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
            using (HealthHelperEntities6 db = new HealthHelperEntities6())
            {
                var list = (from c in db.Comments
                            select new
                            {
                                ID = c.ID,
                                Title = c.Title,
                                Content = c.CommentContent,
                                TargetCommentTitle = c.Post.Title,
                                AddDate = c.AddDate,
                                IsApproved = c.IsApproved
                            }).OrderBy(x => x.AddDate).ToList();
                foreach (var item in list)
                {
                    CommentDTO dto = new CommentDTO();
                    dto.ID = item.ID;
                    dto.Title = item.Title;
                    dto.CommentContent = item.Content;
                    dto.TargetCommentTitle = item.TargetCommentTitle;
                    dto.AddDate = item.AddDate;
                    dto.IsApproved = item.IsApproved;
                    dtoList.Add(dto);
                }
            }
            return dtoList;
        }

        public void AddComment(Comment comment)
        {
            try
            {
                db.Comments.Add(comment);
                db.SaveChanges();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public void DeleteComment(int ID)
        {
            using (HealthHelperEntities6 db = new HealthHelperEntities6())
            {
                Comment comment = db.Comments.First(x => x.ID == ID);
                db.Comments.Remove(comment);
                db.SaveChanges();
            }
        }

        public void ApproveComment(int ID)
        {
            using (HealthHelperEntities6 db = new HealthHelperEntities6())
            {
                Comment comment = db.Comments.First(x => x.ID == ID);
                comment.IsApproved = true;
                db.SaveChanges();
            }
        }

        public List<CommentDTO> GetAllComments(int userID)
        {
            List<CommentDTO> dtoList = new List<CommentDTO>();
            using (HealthHelperEntities6 db = new HealthHelperEntities6())
            {
                var list = (from c in db.Comments
                            where c.MemberID == userID
                            select new
                            {
                                ID = c.ID,
                                Title = c.Title,
                                Content = c.CommentContent,
                                TargetCommentTitle = c.Post.Title,
                                AddDate = c.AddDate,
                                IsApproved = c.IsApproved
                            }).OrderBy(x => x.AddDate).ToList();
                foreach (var item in list)
                {
                    CommentDTO dto = new CommentDTO();
                    dto.ID = item.ID;
                    dto.Title = item.Title;
                    dto.CommentContent = item.Content;
                    dto.TargetCommentTitle = item.TargetCommentTitle;
                    dto.AddDate = item.AddDate;
                    dto.IsApproved = item.IsApproved;
                    dtoList.Add(dto);
                }
            }
            return dtoList;
        }

        public void UpdateComment(LayoutDTO model)
        {
            Comment comment = db.Comments.First(x => x.ID == model.Comment.ID);
            comment.Title = model.Comment.Title;
            comment.CommentContent = model.Comment.CommentContent;
            comment.IsApproved = false;
            db.SaveChanges();
        }

        public List<CommentDTO> GetUnapprovedComments()
        {
            List<CommentDTO> dtoList = new List<CommentDTO>();
            using (HealthHelperEntities6 db = new HealthHelperEntities6())
            {
                var list = db.Comments.Where(x => x.IsApproved == false)
                    .Select(x => new
                    {
                        ID = x.ID,
                        PostTitle = x.Post.Title,
                        Content = x.CommentContent,
                        AddDate = x.AddDate,
                        MemberName = x.Member.Name
                    }).OrderBy(x => x.AddDate).ToList();
                foreach (var item in list)
                {
                    CommentDTO dto = new CommentDTO();
                    dto.ID = item.ID;
                    dto.Title = item.PostTitle;
                    dto.CommentContent = item.Content;
                    dto.AddDate = item.AddDate;
                    dto.MemberName = item.MemberName;
                    dtoList.Add(dto);
                }
            }
            return dtoList;
        }
    }
}
