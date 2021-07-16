using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace DTO
{
    public class TagCategoryDetailDTO
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public int HHOrder { get; set; }
        public HttpPostedFileBase UpLoadImage { get; set; } // Uploaded image
    }
}
