namespace CourseOnline.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class memFix1 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Memberships", "Member_Id", "dbo.Members");
            DropIndex("dbo.Memberships", new[] { "Member_Id" });
            RenameColumn(table: "dbo.Memberships", name: "Member_Id", newName: "MemberId");
            AlterColumn("dbo.Memberships", "MemberId", c => c.Int(nullable: false));
            CreateIndex("dbo.Memberships", "MemberId");
            AddForeignKey("dbo.Memberships", "MemberId", "dbo.Members", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Memberships", "MemberId", "dbo.Members");
            DropIndex("dbo.Memberships", new[] { "MemberId" });
            AlterColumn("dbo.Memberships", "MemberId", c => c.Int());
            RenameColumn(table: "dbo.Memberships", name: "MemberId", newName: "Member_Id");
            CreateIndex("dbo.Memberships", "Member_Id");
            AddForeignKey("dbo.Memberships", "Member_Id", "dbo.Members", "Id");
        }
    }
}
