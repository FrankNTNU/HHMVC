using DTO;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DAL
{
    public class PostDAO : HHContext
    {
        public int AddPost(Post post)
        {
            try
            {
                db.Posts.Add(post);
                db.SaveChanges();
                return post.ID;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public List<PostDTO> GetPosts()
        {
            List<PostDTO> dtoList = new List<PostDTO>();
            var postList = (from p in db.Posts
                            select new
                            {
                                ID = p.ID,
                                Title = p.Title,
                                ShortContent = p.ShortContent,
                                CategoryName = p.PostCategory.Name,
                                AddDate = p.AddDate,
                                MemberID = p.MemberID,
                            }).OrderByDescending(x => x.AddDate).ToList();
            foreach (var item in postList)
            {
                PostDTO dto = new PostDTO();
                dto.Title = item.Title;
                dto.ID = item.ID;
                dto.ShortContent = item.ShortContent;
                dto.CategoryName = item.CategoryName;
                dto.AddDate = item.AddDate;
                dto.MemberID = (int)item.MemberID;
                dto.ImagePath = db.PostImages.First(x => x.PostID == item.ID).ImagePath;
                dtoList.Add(dto);
            }            
            return dtoList;
        }

        public int CountComments(int ID)
        {
            return db.Comments.Where(x => x.PostID == ID && x.IsApproved == true).Count();
        }

        public PostDTO GetPostDetailWithID(int ID)
        {
            Post post = db.Posts.First(x => x.ID == ID);
            PostDTO dto = new PostDTO();
            dto.ID = ID;
            dto.Title = post.Title;
            dto.ShortContent = post.ShortContent;
            dto.PostContent = post.PostContent;
            dto.AddDate = post.AddDate;
            dto.PostImages = GetPostImagesWithPostID(ID);
            dto.CategoryName = post.PostCategory.Name;
            dto.MemberName = post.Member.Name;
            dto.MemberImage = post.Member.Image;
            dto.CommentList = GetCommentsWithPostID(ID);
            return dto;        
        }

        private List<CommentDTO> GetCommentsWithPostID(int postID)
        {
            List<Comment> comments = db.Comments.Where(x => x.PostID == postID && x.IsApproved == true).OrderByDescending(x => x.AddDate).ToList();
            List<CommentDTO> commentDTOList = new List<CommentDTO>();
            foreach (var item in comments)
            {
                CommentDTO dto = new CommentDTO();
                dto.ID = item.ID;
                dto.MemberID = item.MemberID;
                dto.MemberName = item.Member.Name;
                dto.AddDate = item.AddDate;
                dto.Title = item.Title;
                dto.CommentContent = item.CommentContent;
                dto.MemberImage = item.Member.Image;
                dto.IsApproved = item.IsApproved;
                commentDTOList.Add(dto);
            }
            return commentDTOList;
        }

        public string DeletePostImage(int ID)
        {
            try
            {
                string imagePath = "";
                using (HealthHelperEntities db = new HealthHelperEntities())
                {
                    PostImage img = db.PostImages.First(x => x.ID == ID);
                    imagePath = img.ImagePath;
                    db.PostImages.Remove(img);
                    db.SaveChanges();
                }
                return imagePath;

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public List<PostImageDTO> DeletePost(int ID)
        {
            List<PostImageDTO> dtoList = new List<PostImageDTO>();
            using (HealthHelperEntities db = new HealthHelperEntities())
            {
                List<PostImage> imageList = db.PostImages.Where(x => x.PostID == ID).ToList();
                foreach (PostImage item in imageList)
                {
                    PostImageDTO dto = new PostImageDTO();
                    dto.ImagePath = item.ImagePath;
                    dtoList.Add(dto);
                }
                db.PostImages.RemoveRange(imageList);
                Post post = db.Posts.First(x => x.ID == ID);
                db.Posts.Remove(post);
                db.SaveChanges();
            }
            return dtoList;
        }

        public void UpdatePost(PostDTO model)
        {
            using (HealthHelperEntities db = new HealthHelperEntities())
            {
                Post post = db.Posts.First(x => x.ID == model.ID);
                post.Title = model.Title;
                post.ShortContent = model.ShortContent;
                post.PostContent = model.PostContent;
                post.CategoryID = model.CategoryID;
                post.AddDate = DateTime.Now;
                db.SaveChanges();
            }
        }

        public List<PostImageDTO> GetPostImagesWithPostID(int postID)
        {
            List<PostImageDTO> dtoList = new List<PostImageDTO>();
            using (HealthHelperEntities db = new HealthHelperEntities())
            {
                List<PostImage> list = db.PostImages.Where(x => x.PostID == postID).ToList();
                foreach (PostImage item in list)
                {
                    PostImageDTO dto = new PostImageDTO();
                    dto.ID = item.ID;
                    dto.ImagePath = item.ImagePath;
                    dtoList.Add(dto);
                }
            }
            return dtoList;
        }
        public static string StripHTML(string input)
        {
            string noHTML = Regex.Replace(input, @"<[^>]+>|&nbsp;", "").Trim();
            return noHTML;
        }
        public PostDTO GetPostWithID(int ID)
        {
            PostDTO dto = new PostDTO();
            using (HealthHelperEntities db = new HealthHelperEntities())
            {
                Post post = db.Posts.First(x => x.ID == ID);
                dto.ID = post.ID;
                dto.Title = post.Title;
                dto.ShortContent = post.ShortContent;
                dto.PostContent = StripHTML(post.PostContent);
                dto.CategoryID = post.CategoryID;
                dto.MemberName = post.Member.Name;
            }
            return dto;
        }

        public int AddImage(PostImage item)
        {
            try
            {
                db.PostImages.Add(item);
                db.SaveChanges();
                return item.ID;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
