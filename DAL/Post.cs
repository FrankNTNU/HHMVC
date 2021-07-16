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
    
    public partial class Post
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Post()
        {
            this.Comments = new HashSet<Comment>();
            this.LikedPosts = new HashSet<LikedPost>();
            this.PostImages = new HashSet<PostImage>();
        }
    
        public int ID { get; set; }
        public string Title { get; set; }
        public string ShortContent { get; set; }
        public string PostContent { get; set; }
        public int CategoryID { get; set; }
        public int ViewCount { get; set; }
        public Nullable<int> MemberID { get; set; }
        public System.DateTime AddDate { get; set; }
        public int LikeCount { get; set; }
        public bool IsApproved { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Comment> Comments { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<LikedPost> LikedPosts { get; set; }
        public virtual Member Member { get; set; }
        public virtual PostCategory PostCategory { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PostImage> PostImages { get; set; }
    }
}
