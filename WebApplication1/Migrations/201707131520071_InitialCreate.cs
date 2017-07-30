namespace WebApplication1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.WikiComments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Text = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        WikiPage_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.WikiPages", t => t.WikiPage_Id)
                .Index(t => t.WikiPage_Id);
            
            CreateTable(
                "dbo.WikiPages",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PageName = c.String(),
                        PageTitle = c.String(),
                        IsLatest = c.Boolean(nullable: false),
                        ChangedDate = c.DateTime(nullable: false),
                        Content = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.WikiTags",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TagForLink = c.String(maxLength: 40),
                        FullTag = c.String(),
                        WikiPage_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.WikiPages", t => t.WikiPage_Id)
                .Index(t => t.WikiPage_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.WikiComments", "WikiPage_Id", "dbo.WikiPages");
            DropForeignKey("dbo.WikiTags", "WikiPage_Id", "dbo.WikiPages");
            DropIndex("dbo.WikiTags", new[] { "WikiPage_Id" });
            DropIndex("dbo.WikiComments", new[] { "WikiPage_Id" });
            DropTable("dbo.WikiTags");
            DropTable("dbo.WikiPages");
            DropTable("dbo.WikiComments");
        }
    }
}
