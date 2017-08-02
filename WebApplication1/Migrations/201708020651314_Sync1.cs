namespace WebApplication1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Sync1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.WikiPageHistories", "PageTitle", c => c.String());
            AddColumn("dbo.WikiPageHistories", "Content", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.WikiPageHistories", "Content");
            DropColumn("dbo.WikiPageHistories", "PageTitle");
        }
    }
}
