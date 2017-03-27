namespace BusinessSpecificLogic.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class StandarizingReusable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CQAHeader", "is_locked", c => c.Boolean(nullable: false));
            AddColumn("dbo.CQAHeader", "document_status", c => c.String());
            AddColumn("dbo.CQALine", "is_locked", c => c.Boolean(nullable: false));
            AddColumn("dbo.CQALine", "document_status", c => c.String());
            AddColumn("dbo.User", "AuthorizatorPassword", c => c.String(maxLength: 50));
            CreateIndex("dbo.User", "UserName", unique: true);
        }
        
        public override void Down()
        {
            DropIndex("dbo.User", new[] { "UserName" });
            DropColumn("dbo.User", "AuthorizatorPassword");
            DropColumn("dbo.CQALine", "document_status");
            DropColumn("dbo.CQALine", "is_locked");
            DropColumn("dbo.CQAHeader", "document_status");
            DropColumn("dbo.CQAHeader", "is_locked");
        }
    }
}
