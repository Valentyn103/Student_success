namespace Student_success.Models
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

        public virtual DbSet<Group> Groups { get; set; }
        public virtual DbSet<Groups_Subjects> Groups_Subjects { get; set; }
        public virtual DbSet<Mark> Marks { get; set; }
        public virtual DbSet<Student> Students { get; set; }
        public virtual DbSet<Subject> Subjects { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Group>()
                .HasMany(e => e.Groups_Subjects)
                .WithRequired(e => e.Group)
                .HasForeignKey(e => e.GroupsId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Group>()
                .HasMany(e => e.Students)
                .WithRequired(e => e.Group)
                .HasForeignKey(e => e.GroupsId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Student>()
                .HasMany(e => e.Marks)
                .WithRequired(e => e.Student)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Subject>()
                .HasMany(e => e.Groups_Subjects)
                .WithRequired(e => e.Subject)
                .HasForeignKey(e => e.SubjectsId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Subject>()
                .HasMany(e => e.Marks)
                .WithRequired(e => e.Subject)
                .HasForeignKey(e => e.SubjectsId)
                .WillCascadeOnDelete(false);
        }
    }
}
