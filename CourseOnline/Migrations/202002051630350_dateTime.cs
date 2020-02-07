namespace CourseOnline.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class dateTime : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Memberships", "CreatedAt", c => c.DateTime(nullable: true));
            AlterColumn("dbo.Memberships", "UpdatedAt", c => c.DateTime(nullable: true));
            AlterColumn("dbo.Memberships", "DeletedAt", c => c.DateTime(nullable: true));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Memberships", "DeletedAt", c => c.DateTime());
            AlterColumn("dbo.Memberships", "UpdatedAt", c => c.DateTime());
            AlterColumn("dbo.Memberships", "CreatedAt", c => c.DateTime());
        }
    }
}
