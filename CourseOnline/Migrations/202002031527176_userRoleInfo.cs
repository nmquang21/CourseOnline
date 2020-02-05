namespace CourseOnline.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class userRoleInfo : DbMigration
    {
        public override void Up()
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
            
        }
        
        public override void Down()
        {
            DropTable("dbo.UserRoleInformations");
        }
    }
}
