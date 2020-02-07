namespace CourseOnline.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class mem : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Members", "ExpiredYears");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Members", "ExpiredYears", c => c.Int(nullable: false));
        }
    }
}
