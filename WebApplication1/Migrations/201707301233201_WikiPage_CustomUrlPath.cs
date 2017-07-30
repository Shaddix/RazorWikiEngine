namespace WebApplication1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class WikiPage_CustomUrlPath : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.WikiPages", "ViewPath", c => c.String());
            AddColumn("dbo.WikiPages", "ViewUrl", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.WikiPages", "ViewUrl");
            DropColumn("dbo.WikiPages", "ViewPath");
        }
    }
}
