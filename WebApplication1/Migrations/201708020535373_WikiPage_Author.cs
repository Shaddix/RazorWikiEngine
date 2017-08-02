namespace WebApplication1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class WikiPage_Author : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Login = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Login);
            
            AddColumn("dbo.WikiPages", "Author_Login", c => c.String(maxLength: 128));
            CreateIndex("dbo.WikiPages", "Author_Login");
            AddForeignKey("dbo.WikiPages", "Author_Login", "dbo.Users", "Login");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.WikiPages", "Author_Login", "dbo.Users");
            DropIndex("dbo.WikiPages", new[] { "Author_Login" });
            DropColumn("dbo.WikiPages", "Author_Login");
            DropTable("dbo.Users");
        }
    }
}
