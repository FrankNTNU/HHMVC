//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DAL
{
    using System;
    using System.Collections.Generic;
    
    public partial class GiftCart
    {
        public int ID { get; set; }
        public int MemberID { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public System.DateTime AddDate { get; set; }
        public System.DateTime EndDate { get; set; }
        public string Store { get; set; }
        public string Barcode { get; set; }
        public string BarcodeImage { get; set; }
        public int GiftID { get; set; }
    
        public virtual Member Member { get; set; }
    }
}
