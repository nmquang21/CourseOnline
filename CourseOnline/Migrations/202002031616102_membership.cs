namespace CourseOnline.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class membership : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Memberships",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        MebershipRole = c.Int(nullable: false),
                        CreatedAt = c.DateTime(),
                        UpdatedAt = c.DateTime(),
                        DeletedAt = c.DateTime(),
                        ApplicationUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUser_Id)
                .Index(t => t.ApplicationUser_Id);
            
            DropTable("dbo.UserRoleInformations");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.UserRoleInformations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserRoleId = c.String(),
                        CreatedAt = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            DropForeignKey("dbo.Memberships", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropIndex("dbo.Memberships", new[] { "ApplicationUser_Id" });
            DropTable("dbo.Memberships");
        }
    }
}
