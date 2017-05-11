namespace ExpenseTrackingApp.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("FinancialAccountsCategory")]
    public partial class FinancialAccountsCategory
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public FinancialAccountsCategory()
        {
            PersonalAccount = new HashSet<PersonalAccount>();
        }

        [Column(TypeName = "numeric")]
        public decimal ID { get; set; }

        [Required]
        [StringLength(50)]
        public string NameCat { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PersonalAccount> PersonalAccount { get; set; }
    }
}
