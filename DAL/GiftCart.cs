//------------------------------------------------------------------------------
// <auto-generated>
//     這個程式碼是由範本產生。
//
//     對這個檔案進行手動變更可能導致您的應用程式產生未預期的行為。
//     如果重新產生程式碼，將會覆寫對這個檔案的手動變更。
// </auto-generated>
//------------------------------------------------------------------------------

namespace DAL
{
    using System;
    using System.Collections.Generic;
    
    public partial class GiftCart
    {
        public int ID { get; set; }
        public int GiftID { get; set; }
        public int MemberID { get; set; }
        public string Name { get; set; }
        public string Store { get; set; }
        public string Image { get; set; }
        public System.DateTime AddDate { get; set; }
        public System.DateTime EndDate { get; set; }
        public string Barcode { get; set; }
        public string BarcodeImage { get; set; }
    
        public virtual Member Member { get; set; }
    }
}
