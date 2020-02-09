namespace CourseOnline.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class activeCode : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Courses", "ActiveCode", c => c.String(nullable:true));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Courses", "ActiveCode");
        }
    }
}
