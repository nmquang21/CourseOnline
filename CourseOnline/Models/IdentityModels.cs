using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace CourseOnline.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    //public class ApplicationUserRole : IdentityUserRole
    //{
    //    public string baseRoleMemberId { get; set; }
    //    public DateTime CreatedAt { get; set; }
    //    public DateTime UpdatedAt { get; set; }
    //}
    public class ApplicationUser : IdentityUser
    {
        public string firstName { get; set; }
        public string lastName { get; set; }
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
        
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("MyDbContext", throwIfV1Schema: false)
        {
        }
        public DbSet<Category> Categories { set; get; }
        public DbSet<CategoryCourse> CategoryCourses { set; get; }
        public DbSet<Course> Courses { set; get; }
        public DbSet<OrderInfo> OrderInfos { set; get; }
        public DbSet<OrderDetail> OrderDetails { set; get; }
        public DbSet<StudentCourse> StudentCourses { set; get; }
        public DbSet<Benefit> Benefits { set; get; }
        public DbSet<Membership> Memberships { set; get; }
        public DbSet<Member> Members { set; get; }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}