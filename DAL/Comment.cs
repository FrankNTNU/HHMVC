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
    
    public partial class Comment
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string CommentContent { get; set; }
        public int MemberID { get; set; }
        public System.DateTime AddDate { get; set; }
        public bool IsApproved { get; set; }
        public Nullable<int> PostID { get; set; }
        public string Name { get; set; }
        public Nullable<int> ParentCommentID { get; set; }
    
        public virtual Post Post { get; set; }
        public virtual Member Member { get; set; }
    }
}
