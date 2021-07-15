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
    public class GiftDTO
    {
        public int ID { get; set; }
        [Required(ErrorMessage = "請輸入禮物名稱")]
        public string Name { get; set; }
        public string Image { get; set; }
        public HttpPostedFileBase UploadImage { get; set; }
        public int Points { get; set; }
        public int Quantity { get; set; }
        //public string Deadline { get; set; }
        public DateTime AddDate { get; set; }
        [Required(ErrorMessage = "請輸入下架日期")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }
        [Required(ErrorMessage = "請輸入商家名稱")]
        public string Store { get; set; }
        public bool IsUpdate { get; set; } = false;
        public bool IsPremium { get; set; }
    }
}
