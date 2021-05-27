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
    public class UserDTO
    {
        public int ID { get; set; }
        [Required(ErrorMessage = "請輸入使用者名稱")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "請輸入密碼"), DataType(DataType.Password)]
        public string Password { get; set; }
        [Required(ErrorMessage = "請輸入E-mail"), EmailAddress(ErrorMessage = "E-mail格式有誤")]
        public string Email { get; set; }
        public string ImagePath { get; set; }
        [Required(ErrorMessage = "請輸入名稱")]
        public string Name { get; set; }
        public bool isAdmin { get; set; }
        [Required(ErrorMessage = "請輸入身高"), Range(100, 250, ErrorMessage ="請輸入合理的身高數值")]
        public double Height { get; set; }
        [DataType(DataType.Date)]
        
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        [Range(typeof(DateTime), "01/01/1900", "12/31/2020", ErrorMessage = "請輸入合理的日期")]
        [Required(ErrorMessage = "請輸入生日")]
        public DateTime BirthDate { get; set; }
        public DateTime JoinDate { get; set; }
        [Required(ErrorMessage = "請輸入身分證字號")]
        public string TaiwanID { get; set; }
        [Required(ErrorMessage = "請選擇性別")]
        public bool Gender { get; set; }
        [Required(ErrorMessage = "請輸入電話/手機"), DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }
        [Display(Name = "大頭照")]
        public HttpPostedFileBase UserImage { get; set; } // Uploaded image
        [Required(ErrorMessage = "請選擇使用者狀態")]
        public int StatusID { get; set; }
        [Required(ErrorMessage = "請選擇使用者運動強度")]
        public int ActivityLevelID { get; set; }
        public IEnumerable<SelectListItem> Statuses { get; set; }
        public IEnumerable<SelectListItem> ActivityLevels { get; set; }


    }
}
