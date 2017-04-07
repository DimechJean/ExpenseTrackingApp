namespace ExpenseTrackingApp.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TransactionCategory")]
    public partial class TransactionCategory
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public TransactionCategory()
        {
            TransactionPersonal = new HashSet<TransactionPersonal>();
            TransactionOnline = new HashSet<TransactionOnline>();
        }

        [Column(TypeName = "numeric")]
        public decimal ID { get; set; }

        [Required]
        [StringLength(50)]
        public string NameCat { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TransactionPersonal> TransactionPersonal { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TransactionOnline> TransactionOnline { get; set; }
    }
}
