namespace CourseOnline.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fixCourse : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Courses", "TeacherId", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Courses", "TeacherId", c => c.Int(nullable: false));
        }
    }
}
