namespace BusinessSpecificLogic.EF
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using Reusable;

    public partial class CQAContext : DbContext
    {
        public CQAContext()
            : base("name=CQAConn")
        {
            Configuration.LazyLoadingEnabled = false;
            Configuration.ProxyCreationEnabled = false;

            //Database.Log = Console.Write;
        }

        public virtual DbSet<cat_ConcernType> cat_ConcernType { get; set; }
        public virtual DbSet<cat_PartNumber> cat_PartNumber { get; set; }
        public virtual DbSet<cat_ProductLine> cat_ProductLine { get; set; }
        public virtual DbSet<cat_Result> cat_Result { get; set; }
        public virtual DbSet<cat_Status> cat_Status { get; set; }
        public virtual DbSet<CQAHeader> CQAHeaders { get; set; }
        public virtual DbSet<CQALine> CQALines { get; set; }
        public virtual DbSet<CQANumber> CQANumbers { get; set; }
        public virtual DbSet<Customer> Customers { get; set; }

        #region From Reusable Modules
        public virtual DbSet<Track> Tracks { get; set; }
        public virtual DbSet<User> Users { get; set; }
        #endregion

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<cat_PartNumber>()
                .HasMany(e => e.CQAHeaders)
                .WithRequired(e => e.cat_PartNumber)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<CQAHeader>()
                .Property(e => e.ConcertDescription)
                .IsUnicode(false);

            modelBuilder.Entity<CQALine>()
                .Property(e => e.OngoingActivities)
                .IsUnicode(false);

            modelBuilder.Entity<CQANumber>()
                .Property(e => e.GeneratedNumber)
                .IsUnicode(false);

            modelBuilder.Entity<CQANumber>()
                .Property(e => e.TaskDescriptionRevisionReason)
                .IsUnicode(false);

            modelBuilder.Entity<CQANumber>()
                .HasMany(e => e.CQAHeaders)
                .WithRequired(e => e.CQANumber)
                .WillCascadeOnDelete(false);
            
            #region Reusable
            modelBuilder.Entity<User>()
                .Property(e => e.Identicon64)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.Sorts)
                .WithOptional(e => e.User)
                .HasForeignKey(e => e.Sort_User_ID);

            modelBuilder.Entity<User>()
                .HasMany(e => e.Gridsters)
                .WithOptional(e => e.User)
                .HasForeignKey(e => e.Gridster_User_ID);

            modelBuilder.Entity<User>()
                .HasMany(e => e.Tracks)
                .WithOptional(e => e.User_LastEditedBy)
                .HasForeignKey(e => e.User_LastEditedByKey);

            modelBuilder.Entity<User>()
                .HasMany(e => e.Tracks1)
                .WithOptional(e => e.User_RemovedBy)
                .HasForeignKey(e => e.User_RemovedByKey);

            modelBuilder.Entity<User>()
                .HasMany(e => e.Tracks2)
                .WithOptional(e => e.User_AssignedTo)
                .HasForeignKey(e => e.User_AssignedToKey);

            modelBuilder.Entity<User>()
                .HasMany(e => e.Tracks3)
                .WithOptional(e => e.User_AssignedBy)
                .HasForeignKey(e => e.User_AssignedByKey);

            modelBuilder.Entity<User>()
                .HasMany(e => e.Tracks4)
                .WithRequired(e => e.User_CreatedBy)
                .HasForeignKey(e => e.User_CreatedByKey);

            #endregion
        }
    }
}
