namespace WebApplication1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class WikiPage_NewFields : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.WikiPages", "IsSystemPage", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.WikiPages", "IsSystemPage");
        }
    }
}
