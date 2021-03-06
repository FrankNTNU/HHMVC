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
    
    public partial class MealOption
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public MealOption()
        {
            this.DietLogs = new HashSet<DietLog>();
            this.LikedMeals = new HashSet<LikedMeal>();
            this.MealTags = new HashSet<MealTag>();
        }
    
        public int ID { get; set; }
        public string Name { get; set; }
        public double Calories { get; set; }
        public string Image { get; set; }
        public Nullable<int> NutrientID { get; set; }
        public string UnitName { get; set; }
        public string IsVisable { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DietLog> DietLogs { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<LikedMeal> LikedMeals { get; set; }
        public virtual Nutrient Nutrient { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MealTag> MealTags { get; set; }
    }
}
