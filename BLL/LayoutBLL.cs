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
            layoutDTO.Posts = postBLL.GetRecentPosts(6);
            return layoutDTO;
        }

        public LayoutDTO GetPostDetailPageItemWithID(int ID)
        {
            LayoutDTO layoutDTO = new LayoutDTO();
            layoutDTO.PostDetail = postBLL.GetPostDetailPageItemWithID(ID);
            return layoutDTO;
        }
    }
}
