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
    
    public partial class WaterLog
    {
        public int ID { get; set; }
        public int MemberID { get; set; }
        public string Date { get; set; }
        public int WaterAmount { get; set; }
    
        public virtual Member Member { get; set; }
    }
}
