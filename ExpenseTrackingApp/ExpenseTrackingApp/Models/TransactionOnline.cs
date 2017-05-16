namespace ExpenseTrackingApp.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TransactionOnline")]
    public partial class TransactionOnline
    {
        [Column(TypeName = "numeric")]
        public decimal ID { get; set; }

        [Column(TypeName = "numeric")]
        public decimal Amount { get; set; }

        [StringLength(70)]
        public string TransactionDescription { get; set; }

        public decimal TransactionCategory { get; set; }

        public virtual TransactionCategory TransactionCategory1 { get; set; }

        [Column(TypeName = "DateTime"),DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime DateAdded { get; set; }

        [Column(TypeName = "numeric")]
        public decimal Account { get; set; }
    }
}
