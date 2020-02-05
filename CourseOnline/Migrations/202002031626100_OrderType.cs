namespace CourseOnline.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class OrderType : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.OrderInfoes", "orderType", c => c.Int(nullable: true));
        }
        
        public override void Down()
        {
            DropColumn("dbo.OrderInfoes", "orderType");
        }
    }
}
