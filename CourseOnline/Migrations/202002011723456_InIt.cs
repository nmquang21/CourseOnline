namespace CourseOnline.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InIt : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Benefits",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        name = c.String(unicode:true),
                        Course_CourseId = c.Int(),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.Courses", t => t.Course_CourseId)
                .Index(t => t.Course_CourseId);
            
            CreateTable(
                "dbo.Courses",
                c => new
                    {
                        CourseId = c.Int(nullable: false, identity: true),
                        CourseName = c.String(nullable: false, unicode: true),
                        Price = c.Double(nullable: false),
                        Description = c.String(nullable: false, unicode: true),
                        Image = c.String(),
                        TeacherId = c.Int(nullable: false),
                        CreatedAt = c.DateTime(nullable: true),
                        UpdatedAt = c.DateTime(nullable:true),
                        DeletedAt = c.DateTime(nullable: true),
                        ApplicationUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.CourseId)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUser_Id)
                .Index(t => t.ApplicationUser_Id);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        firstName = c.String(unicode: true),
                        lastName = c.String(unicode: true),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.ResourceCourses",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Index = c.Int(nullable: false),
                        Title = c.String(nullable: true, unicode: true),
                        Content = c.String(nullable: true, unicode: true),
                        LinkVideo = c.String(nullable: true),
                        CreatedAt = c.DateTime(nullable: true),
                        UpdatedAt = c.DateTime(nullable: true),
                        DeletedAt = c.DateTime(nullable: true),
                        CourseId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Courses", t => t.CourseId, cascadeDelete: true)
                .Index(t => t.CourseId);
            
            CreateTable(
                "dbo.SubjectCourses",
                c => new
                    {
                        SubjectCourseId = c.Int(nullable: false, identity: true),
                        CourseId = c.Int(nullable: false),
                        SubjectId = c.Int(nullable: false),
                        CreatedAt = c.DateTime(nullable: true),
                        UpdatedAt = c.DateTime(nullable: true),
                        DeletedAt = c.DateTime(nullable: true),
                    })
                .PrimaryKey(t => t.SubjectCourseId)
                .ForeignKey("dbo.Courses", t => t.CourseId, cascadeDelete: true)
                .ForeignKey("dbo.Subjects", t => t.SubjectId, cascadeDelete: true)
                .Index(t => t.CourseId)
                .Index(t => t.SubjectId);
            
            CreateTable(
                "dbo.Subjects",
                c => new
                    {
                        SubjectId = c.Int(nullable: false, identity: true),
                        SubjectName = c.String(unicode: true),
                        Description = c.String(nullable: true, unicode: true),
                        CreatedAt = c.DateTime(nullable: true),
                        UpdatedAt = c.DateTime(nullable: true),
                        DeletedAt = c.DateTime(nullable: true),
                    })
                .PrimaryKey(t => t.SubjectId);
            
            CreateTable(
                "dbo.Favorites",
                c => new
                    {
                        FavoriteId = c.Int(nullable: false, identity: true),
                        SubjectId = c.Int(nullable: false),
                        CreatedAt = c.DateTime(nullable: true),
                        UpdatedAt = c.DateTime(nullable: true),
                        DeletedAt = c.DateTime(nullable: true),
                    })
                .PrimaryKey(t => t.FavoriteId)
                .ForeignKey("dbo.Subjects", t => t.SubjectId, cascadeDelete: true)
                .Index(t => t.SubjectId);
            
            CreateTable(
                "dbo.Categories",
                c => new
                    {
                        CategoryId = c.Int(nullable: false, identity: true),
                        CategoryName = c.String(nullable: false, unicode: true),
                        Description = c.String(nullable: true, unicode: true),
                        CreatedAt = c.DateTime(nullable: true),
                        UpdatedAt = c.DateTime(nullable: true),
                        DeletedAt = c.DateTime(nullable: true),
                    })
                .PrimaryKey(t => t.CategoryId);
            
            CreateTable(
                "dbo.CategoryCourses",
                c => new
                    {
                        CategoryCourseId = c.Int(nullable: false, identity: true),
                        CategoryId = c.Int(nullable: false),
                        CourseId = c.Int(nullable: false),
                        CreatedAt = c.DateTime(nullable: true),
                        UpdatedAt = c.DateTime(nullable: true),
                        DeletedAt = c.DateTime(nullable: true),
                    })
                .PrimaryKey(t => t.CategoryCourseId)
                .ForeignKey("dbo.Categories", t => t.CategoryId, cascadeDelete: true)
                .ForeignKey("dbo.Courses", t => t.CourseId, cascadeDelete: true)
                .Index(t => t.CategoryId)
                .Index(t => t.CourseId);
            
            CreateTable(
                "dbo.OrderDetails",
                c => new
                    {
                        CourseId = c.Int(nullable: false),
                        OrderId = c.String(nullable: false, maxLength: 128),
                        Price = c.Double(nullable: false),
                    })
                .PrimaryKey(t => new { t.CourseId, t.OrderId })
                .ForeignKey("dbo.Courses", t => t.CourseId, cascadeDelete: true)
                .ForeignKey("dbo.OrderInfoes", t => t.OrderId, cascadeDelete: true)
                .Index(t => t.CourseId)
                .Index(t => t.OrderId);
            
            CreateTable(
                "dbo.OrderInfoes",
                c => new
                    {
                        OrderId = c.String(nullable: false, maxLength: 128),
                        MemberId = c.String(),
                        PaymentTypeId = c.Int(nullable: false),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Status = c.Int(nullable: false),
                        CreatedAt = c.String(nullable: true),
                        UpdatedAt = c.String(nullable: true),
                        DeletedAt = c.String(nullable: true),
                        OrderDescription = c.String(nullable: true, unicode: true),
                        BankCode = c.String(nullable: true),
                        vnp_TransactionNo = c.Decimal(nullable: false, precision: 18, scale: 2),
                        vpn_Message = c.String(nullable: true),
                        vpn_TxnResponseCode = c.String(),
                        firstName = c.String(nullable: true, unicode: true),
                        lastName = c.String(nullable: true, unicode: true),
                        phone = c.String(nullable: true),
                        email = c.String(nullable: true),
                        tinh = c.String(nullable: true, unicode: true),
                        quan = c.String(nullable: true, unicode: true),
                        xa = c.String(nullable: true, unicode: true),
                        address = c.String(nullable: true, unicode: true),
                    })
                .PrimaryKey(t => t.OrderId);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                        Description = c.String(nullable: true),
                        CreatedAt = c.DateTime(nullable: true),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.StudentCourses",
                c => new
                    {
                        CourseId = c.Int(nullable: false),
                        MemberId = c.String(nullable: false, maxLength: 128),
                        ApplicationUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => new { t.CourseId, t.MemberId })
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUser_Id)
                .ForeignKey("dbo.Courses", t => t.CourseId, cascadeDelete: true)
                .Index(t => t.CourseId)
                .Index(t => t.ApplicationUser_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.StudentCourses", "CourseId", "dbo.Courses");
            DropForeignKey("dbo.StudentCourses", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.OrderDetails", "OrderId", "dbo.OrderInfoes");
            DropForeignKey("dbo.OrderDetails", "CourseId", "dbo.Courses");
            DropForeignKey("dbo.CategoryCourses", "CourseId", "dbo.Courses");
            DropForeignKey("dbo.CategoryCourses", "CategoryId", "dbo.Categories");
            DropForeignKey("dbo.SubjectCourses", "SubjectId", "dbo.Subjects");
            DropForeignKey("dbo.Favorites", "SubjectId", "dbo.Subjects");
            DropForeignKey("dbo.SubjectCourses", "CourseId", "dbo.Courses");
            DropForeignKey("dbo.ResourceCourses", "CourseId", "dbo.Courses");
            DropForeignKey("dbo.Benefits", "Course_CourseId", "dbo.Courses");
            DropForeignKey("dbo.Courses", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.StudentCourses", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.StudentCourses", new[] { "CourseId" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.OrderDetails", new[] { "OrderId" });
            DropIndex("dbo.OrderDetails", new[] { "CourseId" });
            DropIndex("dbo.CategoryCourses", new[] { "CourseId" });
            DropIndex("dbo.CategoryCourses", new[] { "CategoryId" });
            DropIndex("dbo.Favorites", new[] { "SubjectId" });
            DropIndex("dbo.SubjectCourses", new[] { "SubjectId" });
            DropIndex("dbo.SubjectCourses", new[] { "CourseId" });
            DropIndex("dbo.ResourceCourses", new[] { "CourseId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.Courses", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.Benefits", new[] { "Course_CourseId" });
            DropTable("dbo.StudentCourses");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.OrderInfoes");
            DropTable("dbo.OrderDetails");
            DropTable("dbo.CategoryCourses");
            DropTable("dbo.Categories");
            DropTable("dbo.Favorites");
            DropTable("dbo.Subjects");
            DropTable("dbo.SubjectCourses");
            DropTable("dbo.ResourceCourses");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.Courses");
            DropTable("dbo.Benefits");
        }
    }
}
