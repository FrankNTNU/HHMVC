using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace DTO
{
    public class PostDTO
    {
        public int ID { get; set; }
        [Required(ErrorMessage = "請輸入標題"), MaxLength(50, ErrorMessage = "標題字數不得超過50字")]
        public string Title { get; set; }
        [Required(ErrorMessage = "請輸入大綱"), MaxLength(100, ErrorMessage = "大綱長度不得超過100字")]
        public string ShortContent { get; set; }
        [Required(ErrorMessage = "請輸入內容")]
        public string PostContent { get; set; }
        public int MemberID { get; set; }
        public string MemberName { get; set; }
        [Required(ErrorMessage = "請選擇分類")]
        public int CategoryID { get; set; }
        public string CategoryName { get; set; }
        public IEnumerable<SelectListItem> Categories { get; set; }
        public int ViewCount { get; set; } = 0;
        public int CommentCount { get; set; }
        public List<CommentDTO> CommentList { get; set; }
        public DateTime AddDate { get; set; }
        public string ImagePath { get; set; }
        public List<PostImageDTO> PostImages { get; set; }
        [Display(Name = "圖片")]
        public List<HttpPostedFileBase> PostImage { get; set; }
        public bool IsUpdate { get; set; } = false;
        public string MemberImage { get; set; }
        public int LikeCount { get; set; } = 0;
        public bool IsApproved { get; set; }
    }
    public static class Icons
    {
        public static string Information = "<i class='fas fa-file-alt'></i>";
        public static string Post = "<i class='fas fa-file-alt'></i>";
        public static string Carousel = "<i class='fas fa-book'></i>";
        public static string Notice = "<i class='fas fa-scroll'></i>";
    }

}
