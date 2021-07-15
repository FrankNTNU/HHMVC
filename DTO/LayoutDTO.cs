using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class LayoutDTO
    {
        public List<PostDTO> Posts { get; set; }
        public PostDTO PostDetail { get; set; }
        public CommentDTO Comment { get; set; }
        public int CommentCount { get; set; }
        public List<PostDTO> Carousels { get; set; }
        public CommentDTO CommentRoot { get; set; }

    }
}
