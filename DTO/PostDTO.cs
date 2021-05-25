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
        [Required(ErrorMessage = "請輸入標題")]
        public string Title { get; set; }
        [Required(ErrorMessage = "請輸入大綱")]
        public string ShortContent { get; set; }
        [Required(ErrorMessage = "請輸入內容")]
        public string PostContent { get; set; }
        public int MemberID { get; set; }
        public string MemberName { get; set; }
        [Required(ErrorMessage = "請選擇分類")]
        public int CategoryID { get; set; }
        public string CategoryName { get; set; }
        public IEnumerable<SelectListItem> Categories { get; set; }
        public int ViewCount { get; set; }
        public int CommentCount { get; set; }
        public List<CommentDTO> CommentList { get; set; }
        public DateTime AddDate { get; set; }
        public string ImagePath { get; set; }
        public List<PostImageDTO> PostImages { get; set; }
        [Display(Name = "圖片")]
        public List<HttpPostedFileBase> PostImage { get; set; }
        public bool IsUpdate { get; set; } = false;


    }
}
