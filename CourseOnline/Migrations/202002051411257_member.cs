namespace CourseOnline.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class member : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Members",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        MemberType = c.String(),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Memberships", "Member_Id", c => c.Int());
            CreateIndex("dbo.Memberships", "Member_Id");
            AddForeignKey("dbo.Memberships", "Member_Id", "dbo.Members", "Id");
            DropColumn("dbo.Memberships", "MebershipRole");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Memberships", "MebershipRole", c => c.Int(nullable: false));
            DropForeignKey("dbo.Memberships", "Member_Id", "dbo.Members");
            DropIndex("dbo.Memberships", new[] { "Member_Id" });
            DropColumn("dbo.Memberships", "Member_Id");
            DropTable("dbo.Members");
        }
    }
}
