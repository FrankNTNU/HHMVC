﻿//------------------------------------------------------------------------------
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
    
        public DbSet<ActivityLevel> ActivityLevels { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<DietLog> DietLogs { get; set; }
        public DbSet<GiftCart> GiftCarts { get; set; }
        public DbSet<Gift> Gifts { get; set; }
        public DbSet<GroupChat> GroupChats { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<LikedMeal> LikedMeals { get; set; }
        public DbSet<LikedPost> LikedPosts { get; set; }
        public DbSet<MealOption> MealOptions { get; set; }
        public DbSet<MealTagCategory> MealTagCategories { get; set; }
        public DbSet<MealTag> MealTags { get; set; }
        public DbSet<Member> Members { get; set; }
        public DbSet<ModelCollection> ModelCollections { get; set; }
        public DbSet<Nutrient> Nutrients { get; set; }
        public DbSet<Point> Points { get; set; }
        public DbSet<PostCategory> PostCategories { get; set; }
        public DbSet<PostImage> PostImages { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Program> Programs { get; set; }
        public DbSet<Status> Status { get; set; }
        public DbSet<sysdiagram> sysdiagrams { get; set; }
        public DbSet<Table> Tables { get; set; }
        public DbSet<TempCustomerMealOption> TempCustomerMealOptions { get; set; }
        public DbSet<TimesOfDay> TimesOfDays { get; set; }
        public DbSet<WaterLog> WaterLogs { get; set; }
        public DbSet<WeightLog> WeightLogs { get; set; }
        public DbSet<WorkoutCategory> WorkoutCategories { get; set; }
        public DbSet<WorkoutLog> WorkoutLogs { get; set; }
        public DbSet<WorkoutPreference> WorkoutPreferences { get; set; }
        public DbSet<Workout> Workouts { get; set; }
    }
}
