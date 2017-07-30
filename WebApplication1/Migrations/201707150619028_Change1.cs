namespace WebApplication1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Change1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.WikiPages", "CreatedDate", c => c.DateTimeOffset(nullable: false, precision: 7));
            AlterColumn("dbo.WikiPages", "ChangedDate", c => c.DateTimeOffset(nullable: false, precision: 7));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.WikiPages", "ChangedDate", c => c.DateTime(nullable: false));
            DropColumn("dbo.WikiPages", "CreatedDate");
        }
    }
}
