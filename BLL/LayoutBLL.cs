using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class LayoutBLL
    {
        PostBLL postBLL = new PostBLL();
        public LayoutDTO GetPosts()
        {
            LayoutDTO layoutDTO = new LayoutDTO();
            postBLL = new PostBLL();
            layoutDTO.Posts = postBLL.GetRecentPosts(6);
            layoutDTO.Rules = postBLL.GetRules();
            return layoutDTO;
        }

        public LayoutDTO GetPostDetailPageItemWithID(int ID)
        {
            LayoutDTO layoutDTO = new LayoutDTO();
            postBLL = new PostBLL();
            layoutDTO.PostDetail = postBLL.GetPostDetailPageItemWithID(ID);
            layoutDTO.CommentRoot = postBLL.GetNestedComments(ID);
            layoutDTO.CommentCount = postBLL.CountComments(ID);
            return layoutDTO;
        }
    }
}
