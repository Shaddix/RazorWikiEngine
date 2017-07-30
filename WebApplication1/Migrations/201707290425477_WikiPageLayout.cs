namespace WebApplication1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class WikiPageLayout : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.WikiPages", "IsLayout", c => c.Boolean(nullable: false));
            AddColumn("dbo.WikiPages", "LayoutPage_Id", c => c.Int());
            CreateIndex("dbo.WikiPages", "LayoutPage_Id");
            AddForeignKey("dbo.WikiPages", "LayoutPage_Id", "dbo.WikiPages", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.WikiPages", "LayoutPage_Id", "dbo.WikiPages");
            DropIndex("dbo.WikiPages", new[] { "LayoutPage_Id" });
            DropColumn("dbo.WikiPages", "LayoutPage_Id");
            DropColumn("dbo.WikiPages", "IsLayout");
        }
    }
}
