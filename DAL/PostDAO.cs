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
        int memberID = System.Web.HttpContext.Current.Session["ID"] == null ? 0 : (int)System.Web.HttpContext.Current.Session["ID"];
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

        public List<PostDTO> GetAllPosts()
        {
            List<PostDTO> dtoList = new List<PostDTO>();
            List<PostDTO> postList = (from p in db.Posts
                                      select new PostDTO
                                      {
                                          ID = p.ID,
                                          Title = p.Title,
                                          ShortContent = p.ShortContent,
                                          CategoryName = p.PostCategory.Name,
                                          AddDate = p.AddDate,
                                          MemberID = (int?)p.MemberID ?? 0,
                                          CommentCount = db.Comments.Where(x => x.PostID == p.ID).Count(),
                                          LikeCount = p.LikeCount,
                                          ViewCount = p.ViewCount,
                                          IsApproved = p.IsApproved
                                      }).OrderByDescending(x => x.AddDate).ToList();
            foreach (var item in postList)
            {
                PostDTO dto = new PostDTO();
                dto.Title = item.Title;
                dto.ID = item.ID;
                dto.ShortContent = item.ShortContent;
                dto.CategoryName = item.CategoryName;
                dto.AddDate = item.AddDate;
                dto.MemberID = item.MemberID;
                dto.CommentCount = item.CommentCount;
                dto.ImagePath = db.PostImages.Where(x => x.PostID == item.ID).Select(x => x.ImagePath).DefaultIfEmpty("defaultImg.jpg").First();
                dto.LikeCount = item.LikeCount;
                dto.ViewCount = item.ViewCount;
                dto.IsApproved = item.IsApproved;
                dtoList.Add(dto);
            }
            return dtoList;
        }
        public static CommentDTO Root { get; set; } // 存放結果留言清單
        public CommentDTO GetNestedComments(int postID)
        {
            Root = new CommentDTO();
            CommentDTO tempRoot = Root;
            CurrentLevel = 0;
            PopulateNestedComments(postID, null, ref tempRoot); 
            // 一開始呼叫時只有postID, 沒有指定留言ID
            return Root; 
        }
        private CommentDTO ConvertToModel(Comment comment)
        {
            CommentDTO dto = new CommentDTO();
            dto.ID = comment.ID;
            dto.Title = comment.Title;
            dto.CommentContent = comment.CommentContent;
            dto.MemberID = comment.MemberID;
            dto.Name = comment.Name;
            dto.AddDate = comment.AddDate;
            dto.MemberImage = comment.Member.Image;
            dto.PostID = (int?)comment.PostID ?? 0;
            dto.ParentCommentID = (int?)comment.ParentCommentID ?? 0;
            dto.Level = CurrentLevel;
            dto.IsApproved = comment.IsApproved;
            dto.SentimentScore = comment.SentimentScore;
            return dto;
        }
        private int CurrentLevel = 0;
        public void PopulateNestedComments(int? postID, int? commentID, ref CommentDTO tempRoot)
        {
           
            List<Comment> comments = new List<Comment>();
            tempRoot.ChildComments = new List<CommentDTO>();
            using (HealthHelperEntities db = new HealthHelperEntities())
            {
                if (postID.HasValue && postID != 0)
                // 如果是第一層留言 (postDI != null)
                {
                    Post post = db.Posts.Single(x => x.ID == postID.Value);
                    comments = post.Comments.Where(x => x.ParentCommentID == null).ToList();
                    foreach (Comment item in comments)
                    {
                        var temp = ConvertToModel(item);
                        tempRoot.ChildComments.Add(temp);
                    }
                    // 取得第一層留言 (commentID == null)
                }
                else
                {
                    CurrentLevel++;
                    comments = db.Comments.Where(x => x.ParentCommentID == commentID).ToList();
                    foreach (Comment item in comments)
                    {
                        var temp = ConvertToModel(item);
                        tempRoot.ChildComments.Add(temp);
                    }
                    // 取得第 n 層的留言
                }
            }
            for (int i = 0; i < tempRoot.ChildComments.Count; i++)
            {
                tempRoot.ChildComments[i].ChildComments = new List<CommentDTO>();
                CommentDTO tempComment = tempRoot.ChildComments[i];
                PopulateNestedComments(null, tempRoot.ChildComments[i].ID, ref tempComment);
                CurrentLevel--;
                // 各自去底下找下一層的評論
            }        
        }
        public void DeleteLikedPostsByMemberID(int userID)
        {
            List<LikedPost> likedPosts = db.LikedPosts.Where(x => x.MemberID == userID).ToList();
            db.LikedPosts.RemoveRange(likedPosts);
            db.SaveChanges();
        }

        public void DeletePostsByMemberID(int userID)
        {
            List<Post> posts = db.Posts.Where(x => x.MemberID == userID).ToList();
            db.Posts.RemoveRange(posts);
            db.SaveChanges();
        }

        public List<PostDTO> GetPosts()
        {
            List<PostDTO> dtoList = new List<PostDTO>();

            using (HealthHelperEntities db = new HealthHelperEntities()) {
                List<PostDTO> postList = (from p in db.Posts
                                          where p.IsApproved == true
                                          select new PostDTO
                                          {
                                              ID = p.ID,
                                              Title = p.Title,
                                              ShortContent = p.ShortContent,
                                              CategoryName = p.PostCategory.Name,
                                              AddDate = p.AddDate,
                                              MemberID = (int?)p.MemberID ?? 0,
                                              CommentCount = db.Comments.Where(x => x.PostID == p.ID).Count(),
                                              LikeCount = p.LikeCount,
                                              ViewCount = p.ViewCount,
                                              IsApproved = p.IsApproved
                                          }).OrderByDescending(x => x.AddDate).ToList();
                foreach (var item in postList)
                {
                    PostDTO dto = new PostDTO();
                    dto.Title = item.Title;
                    dto.ID = item.ID;
                    dto.ShortContent = item.ShortContent;
                    dto.CategoryName = item.CategoryName;
                    dto.AddDate = item.AddDate;
                    dto.MemberID = item.MemberID;
                    dto.CommentCount = item.CommentCount;
                    dto.ImagePath = db.PostImages.Where(x => x.PostID == item.ID).Select(x => x.ImagePath).DefaultIfEmpty("defaultImg.jpg").First();
                    dto.LikeCount = item.LikeCount;
                    dto.IsApproved = item.IsApproved;
                    dto.ViewCount = item.ViewCount;
                    dtoList.Add(dto);
                }
            }
            return dtoList;
        }
        public List<PostDTO> GetUserPosts(int userID)
        {
            List<PostDTO> dtoList = new List<PostDTO>();
            var postList = (from p in db.Posts
                            where p.MemberID == userID
                            select new
                            {
                                ID = p.ID,
                                Title = p.Title,
                                MemberID = p.MemberID,
                                ShortContent = p.ShortContent,
                                CategoryName = p.PostCategory.Name,
                                AddDate = p.AddDate,
                                IsApproved = p.IsApproved,
                                ViewCount = p.ViewCount,
                                LikeCount = p.LikeCount,
                                CommentCount = db.Comments.Where(x => x.PostID == p.ID).Count(),
                            }).OrderByDescending(x => x.AddDate).ToList();
            foreach (var item in postList)
            {
                PostDTO dto = new PostDTO();
                dto.Title = item.Title;
                dto.ID = item.ID;
                dto.MemberID = (int)item.MemberID;
                dto.ShortContent = item.ShortContent;
                dto.CategoryName = item.CategoryName;
                dto.AddDate = item.AddDate;
                dto.IsApproved = item.IsApproved;
                dto.ViewCount = item.ViewCount;
                dto.LikeCount = item.LikeCount;
                dto.CommentCount = item.CommentCount;
                dto.ImagePath = db.PostImages.Where(x => x.PostID == item.ID).Select(x => x.ImagePath).DefaultIfEmpty("defaultImg.jpg").First();
                dtoList.Add(dto);
            }
            return dtoList;
        }

        public void AddReply(Comment comment)
        {
            using (HealthHelperEntities db = new HealthHelperEntities())
            {
                db.Comments.Add(comment);
                db.SaveChanges();
            }
        }

        public List<PostDTO> GetPostsByCategory(int categoryID)
        {
            List<PostDTO> dtoList = new List<PostDTO>();
            using (HealthHelperEntities db = new HealthHelperEntities())
            {
                List<int> postIDs = db.Posts.Where(x => x.CategoryID == categoryID && x.IsApproved == true).Select(x => x.ID).ToList();
                foreach (var item in postIDs)
                {
                    PostDTO dto = new PostDTO();
                    dto = GetPostDetailWithID(item);
                    dtoList.Add(dto);
                }
            }
            return dtoList;
        }

       

        public void AddViewCount(int postID)
        {
            using (HealthHelperEntities db = new HealthHelperEntities())
            {
                Post post = db.Posts.FirstOrDefault(x => x.ID == postID);
                post.ViewCount++;
                db.SaveChanges();
            }
        }

        public void BlockPost(int postID)
        {
            using (HealthHelperEntities db = new HealthHelperEntities())
            {
                Post post = db.Posts.FirstOrDefault(x => x.ID == postID);
                post.IsApproved = false;
                db.SaveChanges();
            }
        }

        public int CountComments(int ID)
        {
            return db.Comments.Where(x => x.PostID == ID && x.IsApproved == true).Count();
        }

        public PostDTO GetPostDetailWithID(int ID)
        {
            PostDTO dto = new PostDTO();

            using (HealthHelperEntities db = new HealthHelperEntities())
            {
                Post post = db.Posts.First(x => x.ID == ID);
                dto.ID = ID;
                dto.Title = post.Title;
                dto.ShortContent = post.ShortContent;
                dto.PostContent = post.PostContent;
                dto.AddDate = post.AddDate;
                dto.PostImages = GetPostImagesWithPostID(ID);
                dto.CommentCount = db.Comments.Where(x => x.PostID == ID).Count();
                dto.ImagePath = db.PostImages.Where(x => x.PostID == post.ID).Select(x => x.ImagePath).DefaultIfEmpty("defaultImg.jpg").First();
                dto.CategoryID = post.CategoryID;
                dto.CategoryName = post.PostCategory.Name;
                dto.MemberName = post.Member.Name;
                dto.MemberImage = post.Member.Image;
                dto.MemberID = (int)post.MemberID;
                dto.IsApproved = post.IsApproved;
                dto.LikeCount = post.LikeCount;
                dto.ViewCount = post.ViewCount;
                //dto.CommentList = GetCommentsWithPostID(ID);
            }
            return dto;        
        }

        public bool HasLiked(int userID, int postID)
        {
            using (HealthHelperEntities db = new HealthHelperEntities())
            {
                return db.LikedPosts.FirstOrDefault(x => x.MemberID == userID && x.PostID == postID) != null;
            }
                
        }

        public void LikePost(int postID, int number)
        {
            Post post = db.Posts.First(x => x.ID == postID);
            if (post.LikeCount + number < 0)
            {
                post.LikeCount = 0;
            }
            else
            {
                post.LikeCount += number;
                if (number > 0) // Like a post.
                {
                    LikedPost likedPost = new LikedPost
                    {
                        MemberID = memberID,
                        PostID = postID
                    };
                    db.LikedPosts.Add(likedPost);
                    db.SaveChanges();
                }
                else // Remove a like from the post.
                {
                    LikedPost likedPost = db.LikedPosts.First(x => x.MemberID == memberID && x.PostID == postID);
                    db.LikedPosts.Remove(likedPost);
                    db.SaveChanges();

                }
            }
            
            db.Posts.Attach(post);
            var entry = db.Entry(post);
            entry.State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
        }

        public List<PostDTO> GetPosts(int categoryID, string text)
        {
            List<Post> posts;
            using (HealthHelperEntities db = new HealthHelperEntities())
            {
                if (categoryID != 0 && text == "") // All results under a category.
                {
                    posts = db.Posts.Where(x => x.CategoryID == categoryID && x.IsApproved == true).ToList();
                }
                else if (categoryID == 0 && text != "") // All results matching search text.
                {
                    posts = db.Posts.Where(x => (x.Title.Contains(text) || x.ShortContent.Contains(text)) && x.IsApproved == true).ToList();
                }
                else if (categoryID == 0 && text == "") // All results.
                {
                    posts = db.Posts.Where(x => x.IsApproved == true).ToList();
                }
                else // All results matching both category and search text.
                {
                    posts = db.Posts.Where(x => x.CategoryID == categoryID && (x.Title.Contains(text) || x.ShortContent.Contains(text)) && x.IsApproved == true).ToList();
                }
            }
                
            
            List<PostDTO> postList = new List<PostDTO>();
            foreach (var item in posts)
            {
                PostDTO dto = new PostDTO();
                dto = GetPostDetailWithID(item.ID);
                postList.Add(dto);
            }
            return postList;
        }
        public static int UserPost = 1;
        public static int Information = 2;
        public static int Notice = 3;
        public static int Carousels = 4;
       

        
        //public List<PostDTO> GetNews()
        //{
        //    List<Post> news = db.Posts.Where(x => x.CategoryID == News || x.CategoryID == Notice).ToList();
        //    List<PostDTO> newsList = new List<PostDTO>();
        //    foreach (var item in news)
        //    {
        //        PostDTO dto = new PostDTO();
        //        dto.ID = item.ID;
        //        dto.Title = item.Title;
        //        dto.ShortContent = item.ShortContent;
        //        dto.PostContent = item.PostContent;
        //        dto.CategoryName = item.PostCategory.Name;
        //        dto.ImagePath = db.PostImages.Where(x => x.PostID == item.ID).Select(x => x.ImagePath).DefaultIfEmpty("defaultImg.jpg").First();
        //        dto.AddDate = item.AddDate;
        //        newsList.Add(dto);
        //    }
        //    return newsList;
        //}

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
                dto.Name = item.Name;
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
                DeleteLikedPosts(ID);
                Post post = db.Posts.First(x => x.ID == ID);
                db.Posts.Remove(post);
                db.SaveChanges();
            }
            return dtoList;
        }
        private void DeleteLikedPosts(int postID)
        {
            List<LikedPost> likedPosts = db.LikedPosts.Where(x => x.PostID == postID).ToList();
            db.LikedPosts.RemoveRange(likedPosts);
            db.SaveChanges();
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
                post.IsApproved = model.IsApproved;
                db.Posts.Attach(post);
                var entry = db.Entry(post);
                entry.State = System.Data.Entity.EntityState.Modified;
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
                dto.PostContent = post.PostContent;
                dto.CategoryID = post.CategoryID;
                dto.MemberName = post.Member.Name;
                dto.IsApproved = post.IsApproved;
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
        public List<PostDTO> GetCarousels()
        {
            using (HealthHelperEntities db= new HealthHelperEntities())
            {
                List<PostDTO> dtoList = new List<PostDTO>();
                List<PostDTO> postList = (from p in db.Posts
                                          where p.CategoryID == Carousels
                                          select new PostDTO
                                          {
                                              ID = p.ID,
                                              Title = p.Title,
                                              ShortContent = p.ShortContent,
                                              CategoryName = p.PostCategory.Name,
                                              AddDate = p.AddDate,
                                              MemberID = (int?)p.MemberID ?? 0,
                                              CommentCount = db.Comments.Where(x => x.PostID == p.ID).Count(),
                                              LikeCount = (int?)p.LikeCount ?? 0
                                          }).OrderByDescending(x => x.AddDate).ToList();
                foreach (var item in postList)
                {
                    PostDTO dto = new PostDTO();
                    dto.Title = item.Title;
                    dto.ID = item.ID;
                    dto.ShortContent = item.ShortContent;
                    dto.CategoryName = item.CategoryName;
                    dto.AddDate = item.AddDate;
                    dto.MemberID = item.MemberID;
                    dto.CommentCount = item.CommentCount;
                    dto.ImagePath = db.PostImages.Where(x => x.PostID == item.ID).Select(x => x.ImagePath).DefaultIfEmpty("defaultImg.jpg").First();
                    dto.LikeCount = item.LikeCount;

                    dtoList.Add(dto);
                }
                return dtoList;
            }
        }
        public void ApprovePost(int postID)
        {
            using (HealthHelperEntities db = new HealthHelperEntities()) 
            {
                Post post = db.Posts.FirstOrDefault(x => x.ID == postID);
                post.IsApproved = true;
                db.Posts.Attach(post);
                var entry = db.Entry(post);
                entry.State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }
        }

        public static int GetPostCount() => db.Posts.Count();
        public static int GetUnapprovedCount() => db.Posts.Where(x => x.IsApproved == false).Count();
        public static int GetCarouselCount() => db.Posts.Where(x => x.CategoryID == Carousels).Count();
        public static int GetInfoCount() => db.Posts.Where(x => x.CategoryID == Information).Count();

    }
}
