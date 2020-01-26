using System.Data.Entity;
using StudentHelper.Models;

namespace StudentHelper.Data
{
    public class StudentHelperContext : DbContext
    {
        
        public StudentHelperContext() : base("name=StudentHelperContext")
        {
        }

        public System.Data.Entity.DbSet<Course> Courses { get; set; }
        public System.Data.Entity.DbSet<Staff> Staffs { get; set; }
        public System.Data.Entity.DbSet<User> Users { get; set; }
        public System.Data.Entity.DbSet<Image> Images { get; set; }
        public System.Data.Entity.DbSet<Post> Posts { get; set; }
        public System.Data.Entity.DbSet<Comment> Comments { get; set; }
        public System.Data.Entity.DbSet<UserDetails> UserDetails { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Course>()
                        .HasMany<Staff>(c => c.Professors)
                        .WithMany(p => p.CoursesProf)
                        .Map(pc =>
                        {
                            pc.MapLeftKey("CourseRefId");
                            pc.MapRightKey("StaffRefId");
                            pc.ToTable("Professors");
                        });

            modelBuilder.Entity<Course>()
                        .HasMany<Staff>(c => c.Assistants)
                        .WithMany(a => a.CoursesAssist)
                        .Map(ac =>
                        {
                            ac.MapLeftKey("CourseRefId");
                            ac.MapRightKey("StaffRefId");
                            ac.ToTable("Assistants");
                        });

        }

    }
}
