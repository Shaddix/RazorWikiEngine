namespace WebApplication1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class WikiPageLayout2 : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.WikiPages", name: "LayoutPage_Id", newName: "LayoutPageId");
            RenameIndex(table: "dbo.WikiPages", name: "IX_LayoutPage_Id", newName: "IX_LayoutPageId");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.WikiPages", name: "IX_LayoutPageId", newName: "IX_LayoutPage_Id");
            RenameColumn(table: "dbo.WikiPages", name: "LayoutPageId", newName: "LayoutPage_Id");
        }
    }
}
