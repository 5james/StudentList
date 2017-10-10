namespace StudentListJS
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class StorageContext : DbContext
    {
        public StorageContext(string name = "name=StorageContext")
            : base(name)
        {
        }

        public virtual DbSet<Groups> Groups { get; set; }
        public virtual DbSet<Students> Students { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Groups>()
                .Property(e => e.Stamp)
                .IsFixedLength();

            modelBuilder.Entity<Groups>()
                .HasMany(e => e.Students)
                .WithRequired(e => e.Groups)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Students>()
                .Property(e => e.Stamp)
                .IsFixedLength();
        }
    }
}
