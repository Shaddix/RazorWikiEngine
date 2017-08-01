namespace WebApplication1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class WikiTag_IsSystemTag : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.WikiTags", "IsSystemTag", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.WikiTags", "IsSystemTag");
        }
    }
}
