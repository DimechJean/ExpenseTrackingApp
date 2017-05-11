namespace ExpenseTrackingApp.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("OnlineAccount")]
    public partial class OnlineAccount
    {
        [Column(TypeName = "numeric")]
        public decimal ID { get; set; }

        [StringLength(70)]
        public string AccountDescription { get; set; }

        public string AccountNumber { get; set; }

        [Column(TypeName = "numeric")]
        public decimal UserAccount { get; set; }

        public virtual UserAccount UserAccount1 { get; set; }
    }
}
