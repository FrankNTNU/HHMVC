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
    
        public virtual DbSet<ActivityLevel> ActivityLevels { get; set; }
        public virtual DbSet<CommentCategory> CommentCategories { get; set; }
        public virtual DbSet<Comment> Comments { get; set; }
        public virtual DbSet<DietLog> DietLogs { get; set; }
        public virtual DbSet<LikedMeal> LikedMeals { get; set; }
        public virtual DbSet<MealOption> MealOptions { get; set; }
        public virtual DbSet<MealTagCategory> MealTagCategories { get; set; }
        public virtual DbSet<MealTag> MealTags { get; set; }
        public virtual DbSet<Member> Members { get; set; }
        public virtual DbSet<Nutrient> Nutrients { get; set; }
        public virtual DbSet<Status> Status { get; set; }
        public virtual DbSet<sysdiagram> sysdiagrams { get; set; }
        public virtual DbSet<TimesOfDay> TimesOfDays { get; set; }
        public virtual DbSet<WeightLog> WeightLogs { get; set; }
        public virtual DbSet<WorkoutCategory> WorkoutCategories { get; set; }
        public virtual DbSet<WorkoutLog> WorkoutLogs { get; set; }
        public virtual DbSet<WorkoutPreference> WorkoutPreferences { get; set; }
        public virtual DbSet<Workout> Workouts { get; set; }
        public virtual DbSet<PostCategory> PostCategories { get; set; }
        public virtual DbSet<PostImage> PostImages { get; set; }
        public virtual DbSet<Post> Posts { get; set; }
    }
}