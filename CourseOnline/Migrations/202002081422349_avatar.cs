namespace CourseOnline.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class avatar : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "avatar", c => c.String(nullable:true));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "avatar");
        }
    }
}
