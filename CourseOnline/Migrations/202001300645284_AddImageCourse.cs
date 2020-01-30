namespace CourseOnline.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddImageCourse : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Courses", "Image", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Courses", "Image");
        }
    }
}
