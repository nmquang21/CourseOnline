namespace CourseOnline.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class MyContext : DbContext
    {
        public MyContext()
            : base("name=MyContext")
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
