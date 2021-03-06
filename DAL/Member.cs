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
    
    public partial class Member
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Member()
        {
            this.Comments = new HashSet<Comment>();
            this.DietLogs = new HashSet<DietLog>();
            this.GiftCarts = new HashSet<GiftCart>();
            this.GroupChats = new HashSet<GroupChat>();
            this.LikedMeals = new HashSet<LikedMeal>();
            this.LikedPosts = new HashSet<LikedPost>();
            this.Points1 = new HashSet<Point>();
            this.Posts = new HashSet<Post>();
            this.WorkoutPreferences = new HashSet<WorkoutPreference>();
            this.Programs = new HashSet<Program>();
            this.TempCustomerMealOptions = new HashSet<TempCustomerMealOption>();
            this.WaterLogs = new HashSet<WaterLog>();
            this.WeightLogs = new HashSet<WeightLog>();
            this.WorkoutLogs = new HashSet<WorkoutLog>();
        }
    
        public int ID { get; set; }
        public string TaiwanID { get; set; }
        public string UserName { get; set; }
        public double Height { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public bool Gender { get; set; }
        public bool IsAdmin { get; set; }
        public System.DateTime Birthdate { get; set; }
        public System.DateTime JoinDate { get; set; }
        public string Image { get; set; }
        public int StatusID { get; set; }
        public int ActivityLevelID { get; set; }
        public string Name { get; set; }
        public int Points { get; set; }
        public Nullable<int> MealCount { get; set; }
        public string ActiveCode { get; set; }
        public string GoogleID { get; set; }
        public Nullable<bool> IsVIP { get; set; }
    
        public virtual ActivityLevel ActivityLevel { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Comment> Comments { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DietLog> DietLogs { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<GiftCart> GiftCarts { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<GroupChat> GroupChats { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<LikedMeal> LikedMeals { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<LikedPost> LikedPosts { get; set; }
        public virtual Status Status { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Point> Points1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Post> Posts { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<WorkoutPreference> WorkoutPreferences { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Program> Programs { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TempCustomerMealOption> TempCustomerMealOptions { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<WaterLog> WaterLogs { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<WeightLog> WeightLogs { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<WorkoutLog> WorkoutLogs { get; set; }
    }
}
