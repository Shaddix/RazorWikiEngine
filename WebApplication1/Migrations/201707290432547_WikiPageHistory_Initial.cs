namespace WebApplication1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class WikiPageHistory_Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.WikiPageHistories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ChangedDate = c.DateTimeOffset(nullable: false, precision: 7),
                        WikiPage_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.WikiPages", t => t.WikiPage_Id)
                .Index(t => t.WikiPage_Id);
            
            DropColumn("dbo.WikiPages", "IsLatest");
        }
        
        public override void Down()
        {
            AddColumn("dbo.WikiPages", "IsLatest", c => c.Boolean(nullable: false));
            DropForeignKey("dbo.WikiPageHistories", "WikiPage_Id", "dbo.WikiPages");
            DropIndex("dbo.WikiPageHistories", new[] { "WikiPage_Id" });
            DropTable("dbo.WikiPageHistories");
        }
    }
}
