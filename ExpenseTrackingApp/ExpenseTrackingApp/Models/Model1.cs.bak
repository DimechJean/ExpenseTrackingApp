namespace ExpenseTrackingApp.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class Model1 : DbContext
    {
        public Model1()
            : base("name=Model1")
        {
        }

        public virtual DbSet<FinancialAccountsCategory> FinancialAccountsCategory { get; set; }
        public virtual DbSet<OnlineAccount> OnlineAccount { get; set; }
        public virtual DbSet<PersonalAccount> PersonalAccount { get; set; }
        public virtual DbSet<TransactionCategory> TransactionCategory { get; set; }
        public virtual DbSet<TransactionOnline> TransactionOnline { get; set; }
        public virtual DbSet<TransactionPersonal> TransactionPersonal { get; set; }
        public virtual DbSet<UserAccount> UserAccount { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<FinancialAccountsCategory>()
                .Property(e => e.ID)
                .HasPrecision(18, 0);

            modelBuilder.Entity<FinancialAccountsCategory>()
                .HasMany(e => e.OnlineAccount)
                .WithRequired(e => e.FinancialAccountsCategory)
                .HasForeignKey(e => e.CategoryAcc)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<FinancialAccountsCategory>()
                .HasMany(e => e.PersonalAccount)
                .WithRequired(e => e.FinancialAccountsCategory)
                .HasForeignKey(e => e.CategoryAcc)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<OnlineAccount>()
                .Property(e => e.ID)
                .HasPrecision(18, 0);

            modelBuilder.Entity<OnlineAccount>()
                .Property(e => e.CategoryAcc)
                .HasPrecision(18, 0);

            modelBuilder.Entity<OnlineAccount>()
                .Property(e => e.UserAccount)
                .HasPrecision(18, 0);

            modelBuilder.Entity<PersonalAccount>()
                .Property(e => e.ID)
                .HasPrecision(18, 0);

            modelBuilder.Entity<PersonalAccount>()
                .Property(e => e.CategoryAcc)
                .HasPrecision(18, 0);

            modelBuilder.Entity<PersonalAccount>()
                .Property(e => e.UserAccount)
                .HasPrecision(18, 0);

            modelBuilder.Entity<TransactionCategory>()
                .Property(e => e.ID)
                .HasPrecision(18, 0);

            modelBuilder.Entity<TransactionCategory>()
                .HasMany(e => e.TransactionPersonal)
                .WithRequired(e => e.TransactionCategory1)
                .HasForeignKey(e => e.TransactionCategory)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TransactionCategory>()
                .HasMany(e => e.TransactionOnline)
                .WithRequired(e => e.TransactionCategory1)
                .HasForeignKey(e => e.TransactionCategory)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TransactionOnline>()
                .Property(e => e.ID)
                .HasPrecision(18, 0);

            modelBuilder.Entity<TransactionOnline>()
                .Property(e => e.Amount)
                .HasPrecision(18, 0);

            modelBuilder.Entity<TransactionOnline>()
                .Property(e => e.TransactionCategory)
                .HasPrecision(18, 0);

            modelBuilder.Entity<TransactionPersonal>()
                .Property(e => e.ID)
                .HasPrecision(18, 0);

            modelBuilder.Entity<TransactionPersonal>()
                .Property(e => e.Amount)
                .HasPrecision(18, 0);

            modelBuilder.Entity<TransactionPersonal>()
                .Property(e => e.TransactionCategory)
                .HasPrecision(18, 0);

            modelBuilder.Entity<UserAccount>()
                .Property(e => e.ID)
                .HasPrecision(18, 0);

            modelBuilder.Entity<UserAccount>()
                .HasMany(e => e.OnlineAccount)
                .WithRequired(e => e.UserAccount1)
                .HasForeignKey(e => e.UserAccount)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<UserAccount>()
                .HasMany(e => e.PersonalAccount)
                .WithRequired(e => e.UserAccount1)
                .HasForeignKey(e => e.UserAccount)
                .WillCascadeOnDelete(false);
        }
    }
}
