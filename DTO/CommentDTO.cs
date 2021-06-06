using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class CommentDTO
    {
        public int ID { get; set; }
        [Required(ErrorMessage = "請輸入標題")]
        public string Title { get; set; }
        [Required(ErrorMessage = "請輸入內容")]
        public string CommentContent { get; set; }
        public int MemberID { get; set; }
        public string MemberName { get; set; }
        public DateTime AddDate { get; set; }
        public bool IsApproved { get; set; }
        public string PostTitle { get; set; }
        [Required(ErrorMessage = "請輸入名稱")]
        public string Name { get; set; } // Allow leave a comment anonymously
        public bool IsUpdate { get; set; }
        public string MemberImage { get; set; }

         
    }
}
