using DAL;
using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class PostBLL
    {
        PostDAO postDAO = new PostDAO();
        public List<PostDTO> GetPosts()
        {
            return postDAO.GetPosts();
        }
        public List<PostDTO> GetRecentPosts(int number)
        {
            return postDAO.GetPosts().Take(number).ToList();
        }

        internal List<PostDTO> GetRules()
        {
            return postDAO.GetRules();
        }

        public bool AddPost(PostDTO model)
        {
            Post post = new Post();
            post.Title = model.Title;
            post.ShortContent = model.ShortContent;
            post.PostContent = model.PostContent;
            post.CategoryID = model.CategoryID;
            post.AddDate = DateTime.Now;
            post.MemberID = UserStatic.UserID;
            int ID = postDAO.AddPost(post);
            SavePostImages(model.PostImages, ID);
            return true;
        }

        internal int CountComments(int ID)
        {
            return postDAO.CountComments(ID);
        }

        internal PostDTO GetPostDetailPageItemWithID(int ID)
        {
            return postDAO.GetPostDetailWithID(ID);
        }
        CommentDAO commentDAO = new CommentDAO();
        public bool AddComment(LayoutDTO model)
        {
            Comment comment = new Comment();
            comment.PostID = model.PostDetail.ID;
            comment.MemberID = UserStatic.UserID;
            comment.Name = model.Comment.Name;
            comment.Title = model.Comment.Title;
            comment.CommentContent = model.Comment.CommentContent;
            comment.AddDate = DateTime.Now;
            comment.IsApproved = false;
            comment.Rating = 1;
            commentDAO.AddComment(comment);
            return true;
        }

        void SavePostImages(List<PostImageDTO> list, int PostID)
        {
            List<PostImage> imageList = new List<PostImage>();
            foreach (PostImageDTO item in list)
            {
                PostImage image = new PostImage();
                image.PostID = PostID;
                image.ImagePath = item.ImagePath;
                imageList.Add(image);
            }
            foreach (PostImage item in imageList)
            {
                postDAO.AddImage(item);
            }
        }

        public PostDTO GetPostWithID(int ID)
        {
            PostDTO dto = new PostDTO();
            dto = postDAO.GetPostWithID(ID);
            dto.PostImages = postDAO.GetPostImagesWithPostID(ID);
            return dto;
        }

        public bool UpdatePost(PostDTO model)
        {
            postDAO.UpdatePost(model);
            if (model.PostImages != null)
            {
                SavePostImages(model.PostImages, model.ID);
            }
            return true;
        }

        public bool UpdateComment(LayoutDTO model)
        {
            commentDAO.UpdateComment(model);
            return true;
        }

        public string DeletePostImage(int ID)
        {
            string imagePath = postDAO.DeletePostImage(ID);
            return imagePath;
        }

        public List<PostImageDTO> DeletePost(int ID)
        {
            commentDAO.DeleteCommentByPostID(ID);
            List<PostImageDTO> imageList = postDAO.DeletePost(ID);
            return imageList;
        }
        public List<PostDTO> GetUserPosts(int userID)
        {
            return postDAO.GetUserPosts(userID);
        }

        public List<PostDTO> GetNews()
        {
            return postDAO.GetNews();
        }

        public List<PostDTO> GetPosts(int categoryID, string text)
        {
            return postDAO.GetPosts(categoryID, text);
        }

        public void LikePost(int postID, int number)
        {
            postDAO.LikePost(postID, number);
        }

        public bool HasLiked(int userID, int postID)
        {
            return postDAO.HasLiked(userID, postID);
        }
    }
}
