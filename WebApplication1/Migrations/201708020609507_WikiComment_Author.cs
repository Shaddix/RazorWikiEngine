namespace WebApplication1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class WikiComment_Author : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.WikiComments", "Author_Login", c => c.String(maxLength: 128));
            CreateIndex("dbo.WikiComments", "Author_Login");
            AddForeignKey("dbo.WikiComments", "Author_Login", "dbo.Users", "Login");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.WikiComments", "Author_Login", "dbo.Users");
            DropIndex("dbo.WikiComments", new[] { "Author_Login" });
            DropColumn("dbo.WikiComments", "Author_Login");
        }
    }
}
