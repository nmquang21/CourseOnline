namespace CourseOnline.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class memberAddExpired : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Members", "ExpiredMonths", c => c.Int(nullable: false));
            AddColumn("dbo.Members", "ExpiredYears", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Members", "ExpiredYears");
            DropColumn("dbo.Members", "ExpiredMonths");
        }
    }
}
