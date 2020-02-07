namespace CourseOnline.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class memFix : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Memberships", "UpdatedAt");
            DropColumn("dbo.Memberships", "DeletedAt");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Memberships", "DeletedAt", c => c.DateTime(nullable: false));
            AddColumn("dbo.Memberships", "UpdatedAt", c => c.DateTime(nullable: false));
        }
    }
}
