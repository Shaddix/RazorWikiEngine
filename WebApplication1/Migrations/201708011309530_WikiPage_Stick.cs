namespace WebApplication1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class WikiPage_Stick : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.WikiPages", "StickGlobal", c => c.Boolean(nullable: false));
            AddColumn("dbo.WikiPages", "StickCategory", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.WikiPages", "StickCategory");
            DropColumn("dbo.WikiPages", "StickGlobal");
        }
    }
}
