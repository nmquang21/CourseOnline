namespace CourseOnline.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class courseStatusVideo : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Courses", "IntroduceVideo", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Courses", "IntroduceVideo");
        }
    }
}
