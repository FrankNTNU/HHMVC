using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class CommentDTO
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string CommentTitle { get; set; }
        public string CommentContent { get; set; }
        public int MemberID { get; set; }
        public DateTime AddDate { get; set; }
        public bool IsApproved { get; set; }
        public int MealID { get; set; }
        public int Rating { get; set; }
        public int CategoryID { get; set; }
        public string CategoryName { get; set; }
        public string Feedback { get; set; }
        public int CommentID { get; set; }
        public string TargetCommentTitle { get; set; }

         
    }
}
