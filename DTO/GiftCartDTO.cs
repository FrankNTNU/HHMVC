using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class GiftCartDTO
    {
        public int ID { get; set; }
        public int MemberID { get; set; }
        public string MemberName { get; set; }
        public string Name { get; set; }
        public string Store { get; set; }
        public string Image { get; set; }
        public DateTime AddDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Duration { get; set; }
        public string Barcode { get; set; }
        public string BarcodeImage { get; set; }
    }
}
