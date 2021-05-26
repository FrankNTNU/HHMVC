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
    
    public partial class Nutrient
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Nutrient()
        {
            this.MealOptions = new HashSet<MealOption>();
        }
    
        public int ID { get; set; }
        public double Fat { get; set; }
        public double Protein { get; set; }
        public double Carbs { get; set; }
        public double Sugar { get; set; }
        public double VitA { get; set; }
        public double VitB { get; set; }
        public double VitC { get; set; }
        public double VitD { get; set; }
        public double VitE { get; set; }
        public double Na { get; set; }
        public Nullable<double> Potassium { get; set; }
        public Nullable<double> Calcium { get; set; }
        public Nullable<double> Iron { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MealOption> MealOptions { get; set; }
    }
}