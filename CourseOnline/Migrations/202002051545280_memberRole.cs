namespace CourseOnline.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class memberRole : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Members", "RoleName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Members", "RoleName");
        }
    }
}
