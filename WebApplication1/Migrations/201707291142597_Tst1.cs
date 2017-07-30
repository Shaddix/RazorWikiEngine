namespace WebApplication1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Tst1 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.WikiComments", "CreatedDate", c => c.DateTimeOffset(nullable: false, precision: 7));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.WikiComments", "CreatedDate", c => c.DateTime(nullable: false));
        }
    }
}
