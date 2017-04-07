namespace ExpenseTrackingApp.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Web.Mvc;

    [Table("UserAccount")]
    public partial class UserAccount
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public UserAccount()
        {
            OnlineAccount = new HashSet<OnlineAccount>();
            PersonalAccount = new HashSet<PersonalAccount>();
        }

        [Column(TypeName = "numeric")]
        public decimal ID { get; set; }

        [Required]
        [StringLength(150)]
        [EmailAddress]
        [Display(Name ="Email Address")]
        //[Remote("EmailAddressDB","UserAccounts", HttpMethod = "POST",ErrorMessage = "An Account with the Email Address already exists")]
        public string EmailAcc { get; set; }

        [Required]
        [StringLength(150)]
        [DataType(DataType.Password)]
        [Display(Name ="Password")]
        public string PasswordAcc { get; set; }

        [StringLength(50)]
        [Display(Name ="Name")]
        public string NameAcc { get; set; }

        [StringLength(50)]
        [Display(Name="Surname")]
        public string SurnameAcc { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OnlineAccount> OnlineAccount { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PersonalAccount> PersonalAccount { get; set; }
    }
}
