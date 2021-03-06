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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class HealthHelperEntities : DbContext
    {
        public HealthHelperEntities()
            : base("name=HealthHelperEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<ActivityLevel> ActivityLevels { get; set; }
        public virtual DbSet<Comment> Comments { get; set; }
        public virtual DbSet<DietLog> DietLogs { get; set; }
        public virtual DbSet<GiftCart> GiftCarts { get; set; }
        public virtual DbSet<Gift> Gifts { get; set; }
        public virtual DbSet<GroupChat> GroupChats { get; set; }
        public virtual DbSet<Group> Groups { get; set; }
        public virtual DbSet<LikedMeal> LikedMeals { get; set; }
        public virtual DbSet<LikedPost> LikedPosts { get; set; }
        public virtual DbSet<MealOption> MealOptions { get; set; }
        public virtual DbSet<MealTagCategory> MealTagCategories { get; set; }
        public virtual DbSet<MealTag> MealTags { get; set; }
        public virtual DbSet<Member> Members { get; set; }
        public virtual DbSet<ModelCollection> ModelCollections { get; set; }
        public virtual DbSet<Nutrient> Nutrients { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<Point> Points { get; set; }
        public virtual DbSet<PostCategory> PostCategories { get; set; }
        public virtual DbSet<PostImage> PostImages { get; set; }
        public virtual DbSet<Post> Posts { get; set; }
        public virtual DbSet<Program> Programs { get; set; }
        public virtual DbSet<Status> Status { get; set; }
        public virtual DbSet<sysdiagram> sysdiagrams { get; set; }
        public virtual DbSet<Table> Tables { get; set; }
        public virtual DbSet<TempCustomerMealOption> TempCustomerMealOptions { get; set; }
        public virtual DbSet<TimesOfDay> TimesOfDays { get; set; }
        public virtual DbSet<WaterLog> WaterLogs { get; set; }
        public virtual DbSet<WeightLog> WeightLogs { get; set; }
        public virtual DbSet<WorkoutCategory> WorkoutCategories { get; set; }
        public virtual DbSet<WorkoutLog> WorkoutLogs { get; set; }
        public virtual DbSet<WorkoutPreference> WorkoutPreferences { get; set; }
        public virtual DbSet<Workout> Workouts { get; set; }
    }
}
