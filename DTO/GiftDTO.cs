using System;
using System.Collections.Generic;
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
        public string Name { get; set; }
        public string Image { get; set; }
        public HttpPostedFileBase UpLoadImage { get; set; } // Uploaded image
        public int Point { get; set; }
        public int Quantity { get; set; }
        //public string Deadline { get; set; }
        public DateTime AddDate { get; set; }
        public DateTime EndDate { get; set; }

    }
}
