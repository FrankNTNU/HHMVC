using Azure.AI.TextAnalytics;
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
                                AddDate = c.AddDate,
                                IsApproved = c.IsApproved,
                                MemberName = c.Member.Name,
                                IsReported = c.IsReported,
                                SentimentScore = c.SentimentScore
                            }).OrderBy(x => x.AddDate).ToList();
                foreach (var item in list)
                {
                    CommentDTO dto = new CommentDTO();
                    dto.ID = item.ID;
                    dto.Title = item.Title;
                    dto.CommentContent = item.Content;
                    dto.PostTitle = item.TargetCommentTitle;
                    dto.AddDate = item.AddDate;
                    dto.IsApproved = item.IsApproved;
                    dto.MemberName = item.MemberName;
                    dto.IsReported = item.IsReported;
                    dto.SentimentScore = item.SentimentScore;
                    dtoList.Add(dto);
                }
            }
            return dtoList;
        }

        public void HideComment(int commentID)
        {
            using (HealthHelperEntities db = new HealthHelperEntities())
            {
                Comment comment = db.Comments.FirstOrDefault(x => x.ID == commentID);
                if (comment == null) return;
                comment.IsApproved = false;
                db.Comments.Attach(comment);
                var entry = db.Entry(comment);
                entry.State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }
        }

        public CommentDTO GetComment(int commentID)
        {
            CommentDTO commentDTO = new CommentDTO();
            using (HealthHelperEntities db = new HealthHelperEntities())
            {
                Comment comment = db.Comments.FirstOrDefault(x => x.ID == commentID);
                commentDTO.ID = comment.ID;
                commentDTO.Title = comment.Title;
                commentDTO.MemberID = comment.MemberID;
                commentDTO.Name = comment.Name;
                commentDTO.CommentContent = comment.CommentContent;
            }
            return commentDTO;
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
            using (HealthHelperEntities db = new HealthHelperEntities())
            {
                Comment comment = db.Comments.First(x => x.ID == ID);
                db.Comments.Remove(comment);
                db.SaveChanges();
            }
        }

        public void ApproveComment(int ID)
        {
            using (HealthHelperEntities db = new HealthHelperEntities())
            {
                Comment comment = db.Comments.Find(ID);
                comment.IsApproved = true;
                db.Comments.Attach(comment);
                var entry = db.Entry(comment);
                entry.State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }
        }

        public List<CommentDTO> GetAllComments(int userID)
        {
            List<CommentDTO> dtoList = new List<CommentDTO>();
            using (HealthHelperEntities db = new HealthHelperEntities())
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
                    dto.PostTitle = item.TargetCommentTitle;
                    dto.AddDate = item.AddDate;
                    dto.IsApproved = item.IsApproved;
                    dtoList.Add(dto);
                }
            }
            return dtoList;
        }
        public double GetSentimentScores(CommentDTO comment)
        {
            var client = new TextAnalyticsClient(Constants.endpoint, Constants.credentials);
            string inputText = comment.CommentContent;
            DocumentSentiment documentSentiment = client.AnalyzeSentiment(inputText, language: "zh-hant");
            System.Diagnostics.Debug.WriteLine($"Document sentiment: {documentSentiment.Sentiment}\n");
            System.Diagnostics.Debug.WriteLine($"Document confidence score negative: {documentSentiment.ConfidenceScores.Negative}\n");
            foreach (var sentence in documentSentiment.Sentences)
            {
                System.Diagnostics.Debug.WriteLine($"\tText: \"{sentence.Text}\"");
                System.Diagnostics.Debug.WriteLine($"\tSentence sentiment: {sentence.Sentiment}");
                System.Diagnostics.Debug.WriteLine($"\tPositive score: {sentence.ConfidenceScores.Positive:0.00}");
                System.Diagnostics.Debug.WriteLine($"\tNegative score: {sentence.ConfidenceScores.Negative:0.00}");
                System.Diagnostics.Debug.WriteLine($"\tNeutral score: {sentence.ConfidenceScores.Neutral:0.00}\n");
                
            }
            return documentSentiment.ConfidenceScores.Negative;
        }
        public void UpdateComment(CommentDTO model)
        {
            Comment comment = db.Comments.First(x => x.ID == model.ID);
            comment.Name = model.Name;
            comment.Title = model.Title;
            comment.CommentContent = model.CommentContent;
            comment.IsApproved = true;
            comment.AddDate = DateTime.Today;
            comment.SentimentScore = GetSentimentScores(model);
            db.Comments.Attach(comment);
            db.Entry(comment).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
        }
        public void UpdateSentimentScore(int commentID, double sentimentScore)
        {
            Comment comment = db.Comments.FirstOrDefault(x => x.ID == commentID);
            if (comment == null) return;
            comment.SentimentScore = sentimentScore;
            db.Comments.Attach(comment);
            db.Entry(comment).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
        }
        public List<CommentDTO> GetUnapprovedComments()
        {
            List<CommentDTO> dtoList = new List<CommentDTO>();
            using (HealthHelperEntities db = new HealthHelperEntities())
            {
                var list = db.Comments.Where(x => x.IsApproved == false)
                    .Select(x => new
                    {
                        ID = x.ID,
                        PostTitle = x.Post.Title,
                        Content = x.CommentContent,
                        AddDate = x.AddDate,
                        MemberName = x.Member.Name,
                        x.SentimentScore
                    }).OrderBy(x => x.AddDate).ToList();
                foreach (var item in list)
                {
                    CommentDTO dto = new CommentDTO();
                    dto.ID = item.ID;
                    dto.Title = item.PostTitle;
                    dto.CommentContent = item.Content;
                    dto.AddDate = item.AddDate;
                    dto.MemberName = item.MemberName;
                    dto.SentimentScore = item.SentimentScore;
                    dtoList.Add(dto);
                }
            }
            return dtoList;
        }
        public void DeleteCommentByPostID(int postID)
        {
            List<Comment> comments = db.Comments.Where(x => x.PostID == postID).ToList();
            db.Comments.RemoveRange(comments);
            db.SaveChanges();
        }
        public void ReportComment(int commentID)
        {
            using (HealthHelperEntities db = new HealthHelperEntities())
            {
                Comment comment = db.Comments.FirstOrDefault(x => x.ID == commentID);
                if (comment == null) return;
                if (comment.IsReported == null) comment.IsReported = true;
                if (!(bool)comment.IsReported) comment.IsReported = true;
                db.Comments.Attach(comment);
                db.Entry(comment).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }
        }
        public void ClearReport(int commentID)
        {
            using (HealthHelperEntities db = new HealthHelperEntities())
            {
                Comment comment = db.Comments.FirstOrDefault(x => x.ID == commentID);
                if (comment == null || comment.IsReported == null) return;
                if ((bool)comment.IsReported) comment.IsReported = false;
                db.Comments.Attach(comment);
                db.Entry(comment).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }
        }
        readonly public static double negativeThreshold = 0.8;
        public static int GetCommentCount() => db.Comments.Count();
        public static int GetUnapprovedCount() => db.Comments.Where(x => x.IsApproved == false).Count();
        public static int GetReportedCount() => db.Comments.Where(x => x.IsReported == true).Count();
        public static int GetNegativeCount() => db.Comments.Where(x => x.SentimentScore >= negativeThreshold).Count();



    }
}
