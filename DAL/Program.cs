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
    
    public partial class Program
    {
        public int ID { get; set; }
        public int MemberID { get; set; }
        public int StatusID { get; set; }
        public string Name { get; set; }
        public System.DateTime StartDate { get; set; }
        public System.DateTime EndDate { get; set; }
        public int TargetWeight { get; set; }
        public int ActivityLevelID { get; set; }
        public int InitialWeight { get; set; }
    
        public virtual ActivityLevel ActivityLevel { get; set; }
        public virtual Member Member { get; set; }
        public virtual Status Status { get; set; }
    }
}
