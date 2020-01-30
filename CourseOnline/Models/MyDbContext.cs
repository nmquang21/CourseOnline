namespace CourseOnline.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class MyDbContext : DbContext
    {
        public MyDbContext()
            : base("name=MyDbContext")
        {
        }
        public DbSet<Category> Categories { set; get; }
        public DbSet<CategoryCourse> CategoryCourses { set; get; }
        public DbSet<Course> Courses { set; get; }
        public DbSet<Author> Authors { set; get; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }
}
